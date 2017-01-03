using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DebugLog
{
    public enum eRETURN
    {
        OK,
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

        internal TreeNode Find(char c)
        {
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
        int length = 0;
        public Tree()
        {
        }
        public void AddChildren(int depth, char c, bool last)
        {
            if (slTree.Count <= depth)
            {
                slTree.Add(new TreeNode());
            }
            TreeNode tn = new TreeNode();
            switch (decodeFormat)
            {
                case eDecodeFormat.CHAR:
                    if (c == '%')
                    {
                        Trace.WriteLineIf(debug > 1, "Found %", "AC");
                        length = 0;
                        decodeFormat = eDecodeFormat.LENGTH;
                    }
                    else
                    {
                        tn = new TreeNode(c, last);
                    }
                    break;
                case eDecodeFormat.LENGTH:
                    if ((c >= '0') && (c <= 9))
                    { // is Lenght 0-9
                        length = c - 0x30; // is this safe?
                        decodeFormat = eDecodeFormat.KIND;
                    }
                    else
                    {
                        Trace.WriteLine("Error in Codes", "AC");
                        throw new Exception("Error " + c);
                    }
                    break;
                case eDecodeFormat.KIND:
                    tn = new TreeNode(c, eKind.INT, length, last);
                    decodeFormat = eDecodeFormat.CHAR;
                    break;
                default:
                    break;
            }
            TreeNode t = slTree[depth].Find(c);
            Trace.Write("Add " + depth + "\t " + c);
            if (t != null)
            {
                Trace.WriteLine("\tfound, Replace current");
                current = t;
            }
            else
            {
                Trace.WriteLine("\tnot found, add new Node");
                slTree[depth].Add(tn);
            }
        }

        public TreeNode Find(int depth, char c)
        {
            Debug.Assert(slTree.Count > depth);
            return slTree[depth].Find(c);
        }
    }
}
