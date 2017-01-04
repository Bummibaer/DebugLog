using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DebugLog
{
    public enum eRETURN
    {
        OK,
        NEXT,
        DECODED,
        ERROR
    }
    public enum eKind
    {
        CHAR,
        INT,
        FLOAT
    }

    public class TreeNode : IEquatable<TreeNode>
    {
        List<TreeNode> lNodes = new List<TreeNode>();
        TreeNode previous;
        bool bLast;
        public char value;
        public eKind kind;
        public int length;

        public TreeNode(char c)
        {
            this.value = c;
            length = 0;
            this.kind = eKind.CHAR;
            bLast = false;
        }

        public TreeNode(char c, bool last)
        {
            this.value = c;
            length = 0;
            this.kind = eKind.CHAR;
            bLast = last;
        }

        public TreeNode(char c, eKind kind, int length)
        {
            this.value = c;
            this.length = length;
            this.kind = kind;
            bLast = false;
        }

        public TreeNode(char c, eKind kind, int length, bool last)
        {
            this.value = c;
            this.length = length;
            this.kind = kind;
            bLast = last;
        }

        public bool Equals(TreeNode other)
        {
            return value == other.value;
        }

        int debug = 2;

        public bool Last
        {
            get
            {
                return bLast;
            }
        }

        public TreeNode(TreeNode treenode, char c)
        {
            previous = treenode;
            value = c;
        }

        public TreeNode()
        {

        }

        int count_length = 0;
        internal TreeNode Find(char c)
        {
            foreach (TreeNode tn in lNodes)
            {
                if (tn.kind == eKind.CHAR)
                {
                    if (tn.value == c)
                        return tn;
                }
                else
                {
                    if (tn.length < count_length)
                    {
                    }
                }
            }
            return lNodes.Find(x => x.value.Equals(c));
        }

        internal void Add(TreeNode c)
        {
            lNodes.Add(c);
        }

    }

    public class Tree
    {
        static List<TreeNode> slTree = new List<TreeNode>();
        TreeNode current;
        int debug = 2;
        enum eDecodeFormat
        {
            CHAR,
            LENGTH,
            KIND
        };

        eDecodeFormat decodeFormat = eDecodeFormat.CHAR;
        TreeNode tn = new TreeNode();

        int tDepth = 0;
        public Tree()
        {
        }
        public void AddChildren(char c, bool last)
        {
            if (slTree.Count <= tDepth)
            {
                slTree.Add(new TreeNode());
            }
            switch (decodeFormat)
            {
                case eDecodeFormat.CHAR:
                    tn = new TreeNode(c, last);
                    if (c == '%')
                    {
                        Trace.WriteLineIf(debug > 1, "Found %", "AC");
                        decodeFormat = eDecodeFormat.LENGTH;
                    }
                    else
                    {
                        TreeNode t = slTree[tDepth].Find(c);
                        Trace.Write("Add " + tDepth + "\t " + c);
                        if (t != null)
                        {
                            Trace.WriteLine("\tfound, Replace current");
                            current = t;
                        }
                        else
                        {
                            Trace.WriteLine("\tnot found, add new Node");
                            slTree[tDepth].Add(tn);
                        }
                        tDepth++;
                    }
                    break;
                case eDecodeFormat.LENGTH:
                    if ((c >= '0') && (c <= '9'))
                    { // is Lenght 0-9
                        tn.length = c - 0x30; // is this safe?
                        Trace.WriteLineIf(debug > 1, "Length = " + tn.length, "AC");
                        decodeFormat = eDecodeFormat.KIND;
                    }
                    else
                    {
                        Trace.WriteLine("Error in Codes", "AC");
                        throw new Exception("Error " + c);
                    }
                    break;
                case eDecodeFormat.KIND:
                    Trace.WriteLineIf(debug > 1, "New Type " + c, "AC");
                    switch (c)
                    {
                        case 'd':
                            tn.kind = eKind.INT;
                            break;
                        case 'f':
                            tn.kind = eKind.FLOAT;
                            break;
                        default:
                            throw new Exception("Unknown type specifier : " + c);
                            break;
                    }
                    slTree[tDepth].Add(tn);  /// TODO nicht eindeutig?
                    tDepth++;
                    decodeFormat = eDecodeFormat.CHAR;
                    break;
                default:
                    break;
            }
            if (last) tDepth = 0;
        }

        public TreeNode Find(int depth, char c)
        {
            Debug.Assert(slTree.Count > depth);
            return slTree[depth].Find(c);
        }
    }
}
