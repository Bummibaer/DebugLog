using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLog
{
    public enum eRETURN
    {
        OK,
        DECODED,
        ERROR
    }

    class TreeNode
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

        private int depth = 0;

        internal eRETURN Expect(char c)
        {
            eRETURN rc = eRETURN.OK;
            Trace.Write("Expect " + c + "\t");
            Parser.sNode sn = new Parser.sNode(c);
            TreeNode tn;
            if ((tn = slTree[depth].Find(sn)) != null)
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
