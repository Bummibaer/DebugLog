using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Text;
//using System.Threading.Tasks;

namespace DebugLog
{
    public class Parser
    {
        /// <summary>
        /// literal
        /// %nd
        /// %nf n float
        /// %% arbitrary Literal
        /// 
        /// </summary>
        const char cFLOAT = (char)0xFF;
        const char cINT = (char)0xFE;

        Queue<char> qChars = new Queue<char>();
        enum eDecode
        {
            IDLE,
            TYPE,
            LENGTH,
            DECODING
        };
        char type;
        int length;
        eDecode sDecode = eDecode.IDLE;
        byte[] value = new byte[4];
        int index = 0;


        private Tree tree;

        public Parser(string[] codes)
        {
            tree = new Tree();
            CreateTree(codes);
        }

        bool CreateTree(string[] codes)
        {
            Tree tree = new Tree();
            foreach (string s in codes)
            {
                char[] cs = s.ToCharArray();
                for (int i = 0; i < s.Length; i++)
                {
                    tree.AddChildren(i, cs[i], i == (s.Length - 1));
                }

            }
            return true;
        }

        public eRETURN Parse(char c)
        {
            eRETURN er = Expect(c);
            qChars.Enqueue(c);
            switch (er)
            {
                case eRETURN.DECODED:
                    qChars.Clear();
                    Trace.WriteLine("Parser successfull !");
                    break;
                case eRETURN.ERROR:
                    Trace.Write("ERROR !\t");
                    qChars.Dequeue();
                    while (qChars.Count > 0)
                    {
                        Trace.WriteLine("Try again!");
                        Parse(qChars.Dequeue());
                    }
                    break;
                default:
                    switch (sDecode)
                    {
                        case eDecode.IDLE:
                            if (c == '%') sDecode = eDecode.TYPE;
                            break;
                        case eDecode.TYPE:
                            type = c;
                            sDecode = eDecode.LENGTH;
                            break;
                        case eDecode.LENGTH:
                            if (!int.TryParse(c.ToString(), out length))
                            {
                                sDecode = eDecode.IDLE;
                                qChars.Clear();
                                Trace.WriteLine("Couldn't decode : " + c);
                            }
                            else
                            {
                                sDecode = eDecode.DECODING;
                                index = 0;
                            }
                            break;
                        case eDecode.DECODING:
                            value[index++] = (byte)c;
                            if (index >= length)
                            {
                                switch (type)
                                {
                                    case 'd':
                                        BitConverter.ToUInt16(value, 0);
                                        break;
                                    case 'f':
                                        throw new NotImplementedException("Float");
                                        break;
                                    default:
                                        throw new NotImplementedException("Type:" + type);
                                        break;
                                }
                                sDecode = eDecode.IDLE;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return er;
        }

        private int depth = 0;

        internal eRETURN Expect(char c)
        {
            eRETURN rc = eRETURN.OK;
            Trace.Write("Expect " + c + "\t");
            TreeNode tn;
            if ((tn = tree.Find(depth, c)) != null)
            {
                depth++;
                if (tn.Last)
                {
                    depth = 0;
                    rc = eRETURN.DECODED;
                }
            }
            else
            {
                depth--;
                if (depth < 0) depth = 0;
                rc = eRETURN.ERROR;

            }
            return rc;
        }
    }

}

