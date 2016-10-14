using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MmCifSL
{
    public class MmCifParser
    {
        public MmCifParser() { }

        public MmCifData Parse(string s)
        {
            var tokenizer = new MmCifTokenizer(s);

            string token;
            MmCifTokenizer.TokenType tokenType;
            MmCifData data;

            // read first token
            // must be data_*

            if (tokenizer.nextToken(out token, out tokenType))
            {
                if (tokenType == MmCifTokenizer.TokenType.NonQuoted && token.ToLower().StartsWith("data_"))
                {
                    data = new MmCifData(token.Substring(5)); // copy text after "data_" (index 5)
                }
                else
                {
                    throw new MmCifParseException("Does not start with data_ block");
                }
            }
            else
            {
                throw new MmCifParseException("No data_ block found");
            }

            // read rest of file

            ParsingStatus status = ParsingStatus.Idle;
            MmCifTag tag = null;
            MmCifDataCategory loopCategory = null;
            List<MmCifTag> loopTags = new List<MmCifTag>();
            int loopTagIndex = 0;

            while (tokenizer.nextToken(out token, out tokenType))
            {
                bool tokenHandled = false;
                int tokenHandledTtl = 100;

                while (!tokenHandled)
                {
                    switch (status)
                    {
                        case ParsingStatus.Idle:

                            if (isTagName(token, tokenType))
                            {
                                // start of DataItem
                                tag = new MmCifTag(token);

                                status = ParsingStatus.DataItem;
                                tokenHandled = true;
                            }
                            else if (isLoop(token, tokenType))
                            {
                                loopTags.Clear();

                                status = ParsingStatus.LoopDefinition;
                                tokenHandled = true;
                            }
                            else
                            {
                                throw new MmCifParseException("Unexpected value");
                            }

                            break;
                        case ParsingStatus.DataItem:
                            var categoryData = new MmCifDataCategory(tag.Category);
                            categoryData[tag.Attribute] = token;
                            data[tag.Category].Add(categoryData);

                            tag = null;

                            status = ParsingStatus.Idle;
                            tokenHandled = true;
                            break;
                        case ParsingStatus.LoopDefinition:

                            if (isTagName(token, tokenType))
                            {
                                tag = new MmCifTag(token);

                                // ensure all tags have the same category
                                foreach (MmCifTag existingTag in loopTags)
                                {
                                    if (existingTag.Category != tag.Category)
                                    {
                                        throw new MmCifParseException("all tags in a loop_ must have the same category");
                                    }
                                }

                                // add to list
                                loopTags.Add(tag);

                                tag = null;
                                tokenHandled = true;
                            }
                            else
                            {
                                // this is the first item

                                if (loopTags.Count < 1)
                                {
                                    throw new MmCifParseException("there are no data tags for the loop_ defined. At least 1 must be defined");
                                }

                                loopTagIndex = loopTags.Count; // so it sarts with a new "line" in Loop - status

                                status = ParsingStatus.Loop;
                                tokenHandled = false; // handle this properly in next Loop-state
                            }
                            break;
                        case ParsingStatus.Loop:

                            if (isTagName(token, tokenType) || isLoop(token, tokenType))
                            {
                                // no data value. seems like loop is over
                                if (loopTagIndex != loopTags.Count - 1)
                                {
                                    throw new MmCifParseException("there is not a value for each column in loop_");
                                }

                                loopTags.Clear();

                                status = ParsingStatus.Idle;
                                tokenHandled = false; // handle in Idle
                            }
                            else
                            {
                                loopTagIndex++;
                                if (loopTagIndex >= loopTags.Count)
                                {
                                    loopTagIndex = 0;

                                    loopCategory = new MmCifDataCategory(loopTags[0].Category);
                                    data[loopCategory.CategoryName].Add(loopCategory);
                                }

                                tag = loopTags[loopTagIndex];
                                loopCategory[tag.Attribute] = token;

                                tag = null;
                                tokenHandled = true;
                            }
                            break;
                        default:
                            throw new MmCifParseException("internal error: unknown state " + status.ToString());
                    }

                    if (tokenHandledTtl-- <= 0)
                    {
                        throw new MmCifParseException("internal error: token could not be handled properly: token=" + token + ", type=" + tokenType.ToString());
                    }
                }
            }

            return data;
        }

        private bool isTagName(string token, MmCifTokenizer.TokenType tokenType)
        {
            return (tokenType == MmCifTokenizer.TokenType.NonQuoted && token.StartsWith("_"));
        }

        private bool isLoop(string token, MmCifTokenizer.TokenType tokenType)
        {
            return (tokenType == MmCifTokenizer.TokenType.NonQuoted && token.ToLower() == "loop_");
        }

        private enum ParsingStatus
        {
            Idle,
            DataItem,
            LoopDefinition,
            Loop
        }
    }
}
