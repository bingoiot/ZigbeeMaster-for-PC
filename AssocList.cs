using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MySerialPorts
{
    class AssocList
    {
        public class Tree_t
        {
            public UInt16 addr;
            public byte lqi;
            public byte devType;
            public Boolean finished;
            public List<Tree_t> child;
        }
        private static Tree_t   myTree = null;
        private static Mutex    myMutex = new Mutex();

        public void MessageInput(byte[] data, int len)
        {
            UInt16 srcAddr;
            int index = 4;//start of data
            srcAddr = ZigbeeCommon.BtoU16(data, index); index += 2;
            if (data[index++] == 0x00)//if succees
            {
                byte totalNum = data[index++];
                byte startDevID = data[index++];
                byte curNum = data[index++];
                for (int i = 0; i < curNum; i++)
                {
                    index += 16;
                    UInt16 DevAddr = ZigbeeCommon.BtoU16(data, index);
                    index += 2;
                    byte devType = (byte)((data[index] & 0x03) + 1);
                    index += 3;
                    byte lqi = data[index++];
                    if ((devType == 0x02)&&(CheckFinished(DevAddr)==false))
                        ZigbeeApi.Instance.ReqAssoc(DevAddr, 0);
                    if (CheckExist(DevAddr) == false)
                    { 
                        Tree_t node = new Tree_t();
                        node.addr = DevAddr;
                        node.devType = devType;
                        node.child = null;
                        node.lqi = lqi;
                        InsertTree(srcAddr, node);
                    }
                    if ((DevAddr != 0x0000) && (ZigbeeApi.Device.CheckExist(DevAddr) == false))
                    {
                        ZigbeeApi.Device.AddDevice(DevAddr, null);
                    }
                }
                if ((startDevID + curNum) < totalNum)
                    ZigbeeApi.Instance.ReqAssoc(srcAddr, (byte)(startDevID + curNum));
                else
                    SetFinishedFlag(srcAddr,true);
            }
        }
        public void LockList()
        {
            myMutex.WaitOne();
        }
        public Tree_t GetAssocList()
        {
            return myTree;
        }
        public void ReleaseList()
        {
            myMutex.ReleaseMutex();
        }
        public void ClearList()
        {
            myTree = null;
        }
        private void InsertTree(UInt16 addr, Tree_t node)
        {
            myMutex.WaitOne();
            if (myTree == null)
            {
                Tree_t n = new Tree_t();
                n.addr = 0x0000;
                n.devType = ZigbeeCommon.DevTpye_Coord;
                n.child = new List<Tree_t>();
                myTree = n;
                myTree.child.Add(node);
            }
            else
            {
                Tree_t father = GetNode(addr, myTree);
                if (father != null)
                {
                    if (father.child == null)
                        father.child = new List<Tree_t>();
                    father.child.Add(node);
                }
            }
            myMutex.ReleaseMutex();
        }
        private void Remove(UInt16 addr)
        {
            myMutex.WaitOne();
            if (myTree != null)
            {
                Tree_t father = GetFather(addr, myTree);
                if (father != null)
                {
                    if (father.child != null)
                    {
                        for (int i = 0; i < father.child.Count; i++)
                        {
                            if (father.child[i].addr == addr)
                                father.child.RemoveAt(i);
                        } 
                    }
                }
            }
            myMutex.ReleaseMutex();
        }
        private Boolean CheckExist(UInt16 addr)
        {
            Boolean flag = false;
            myMutex.WaitOne();
            if (myTree != null)
            {
                Tree_t tree = GetNode(addr, myTree);
                if (tree != null)
                    flag = true;
            }
            myMutex.ReleaseMutex();
            return flag;
        }
        private Boolean CheckFinished(UInt16 addr)
        {
            Boolean flag = false;
            myMutex.WaitOne();
            if (myTree != null)
            {
                Tree_t tree = GetNode(addr, myTree);
                if ((tree != null)&&(tree.finished==true))
                    flag = true;
            }
            myMutex.ReleaseMutex();
            return flag;
        }
        private void SetFinishedFlag(UInt16 addr, Boolean flag)
        {
            myMutex.WaitOne();
            if (myTree != null)
            {
                Tree_t tree = GetNode(addr, myTree);
                if (tree != null)
                    tree.finished = flag;
            }
            myMutex.ReleaseMutex();
        }
        private Tree_t GetNode(UInt16 addr, Tree_t tree)
        {
            if (tree.addr == addr)
                return tree;
            else if(tree.child!=null)
            {
                for (int i = 0; i < tree.child.Count; i++)
                {
                    Tree_t node = GetNode(addr,tree.child[i]);
                    if (node != null)
                        return node;
                }      
            }
            return null;
        }
        private Tree_t GetFather(UInt16 addr, Tree_t tree)
        {
            if (tree.child != null)
            {
                for (int i = 0; i < tree.child.Count; i++)
                {
                    if (tree.child[i].addr == addr)
                        return tree;
                    else
                    {
                        Tree_t node = GetFather(addr, tree.child[i]);
                        if (node != null)
                            return tree;
                    }
                }
            }
            return null;
        }


    }
}
