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

        public struct sNode : IEquatable<sNode>
        {
            public bool last;
            public char value;

            public sNode(char c)
            {
                this.value = c;
                last = false;
            }

            public sNode(char c, bool last)
            {
                this.value = c;
                this.last = last;
            }

            public bool Equals(sNode other)
            {
                return value == other.value;
            }
        }

        private Tree tree;

        public Parser(Tree tree)
        {
            this.tree = tree;
        }

        public eRETURN CreateTree(char c)
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
                        CreateTree(qChars.Dequeue());
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
                                        throw new NotImplementedException("Float");
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
            Parser.sNode sn = new Parser.sNode(c);
            TreeNode tn;
            if ((tn = tree.Find(depth,sn)) != null)
            {
                depth++;
                if (tn.Value.last)
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

