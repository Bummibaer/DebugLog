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

    public class TreeNode
    {
        List<TreeNode> lNodes = new List<TreeNode>();
        TreeNode previous;
        Parser.sNode value;

        public Parser.sNode Value
        {
            get
            {
                return value;
            }
        }

        public TreeNode(TreeNode tree, Parser.sNode c)
        {
            previous = tree;

            value = c;
        }

        public TreeNode()
        {

        }

        internal TreeNode Find(Parser.sNode c)
        {
            return lNodes.Find(x => x.value.Equals(c));
        }

        internal void Add(Parser.sNode c)
        {
            lNodes.Add(new TreeNode(this, c));
        }
    }

    public class Tree
    {
        static List<TreeNode> slTree = new List<TreeNode>();
        TreeNode current;

        public Tree()
        {
        }
        public void AddChildren(int depth, Parser.sNode c)
        {
            if (slTree.Count <= depth)
            {
                slTree.Add(new TreeNode());
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
                slTree[depth].Add(c);
            }
        }

        public TreeNode Find(int depth, Parser.sNode sn)
        {
            Debug.Assert(slTree.Count > depth);
            return slTree[depth].Find(sn);
        }
    }
}
