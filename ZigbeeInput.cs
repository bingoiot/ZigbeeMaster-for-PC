using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySerialPorts
{
    class ZigbeeInput
    {
        private const int RECEIVE_BUF_SIZE = 1024;
        static byte[]   data_buf = new byte[RECEIVE_BUF_SIZE];
        static int     fifo_counter = 0;
        public void MessageInput(byte[] data, int len)
        {
            int remain, temp;
            temp = fifo_counter + len;
            if (temp > RECEIVE_BUF_SIZE)
            {
                remain = temp - RECEIVE_BUF_SIZE;
                fifo_counter -= remain;
                ZigbeeCommon.Memcpy(data_buf,remain,data_buf,0,fifo_counter);
            }
            Array.Copy(data,0,data_buf,fifo_counter,len);
            fifo_counter += len;
        }
        public byte[] read_package()
        {
            int remain;
            int index = 0;
            int slen;
            while (index < fifo_counter)
            {
                remain = (fifo_counter - index);
                index = find_sof(data_buf, index, remain);
                if (index >= 0)
                {
                    if (check_package(data_buf,index, remain) == 0)
                    {
                        slen = data_buf[index + 1] + 5;//total package length
                        byte[] buf = new byte[slen];
                        Array.Copy(data_buf,index,buf,0,slen);
                        fifo_counter -= slen;
                        int clen = fifo_counter - index;
                        if ((clen >= RECEIVE_BUF_SIZE) || (clen < 0))
                            fifo_counter = 0;
                        else 
                            ZigbeeCommon.Memcpy(data_buf,(index + slen),data_buf,index, clen);
                        return buf;
                    }
                    else
                    {
                        index++;//move to next byte
                    }
                }
                else
                {
                    break;
                }
            }
            return null;
        }
        private int find_sof(byte[] pdata, int startIndex, int len)
        {
            int i;
            for (i = 0; i < len; i++)
            {
                if (pdata[i + startIndex] == 0xFE)
                    return (i + startIndex);
            }
            return -1;
        }
 
        private int check_package(byte[] pdata,int startID, int len)
        {
            int slen;
            byte fcs, sum;
            if (len < 5)
                return 1;
            slen = (pdata[startID+1] + 5);
            if (slen > len)//package length error!
            {
                return 1;
            }
            fcs = pdata[startID+(slen - 1)];
            sum = ZigbeeCommon.GetXor(pdata,startID+1,(slen - 2));
            if (fcs == sum)
                return 0;
            else
                return 1;
        }
    }
}
