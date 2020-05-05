using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Utilities
{
    public static class GetNextId
    {
        public static string NextID(string lastID, string prefixID)
        {
            if (lastID == "")
            {
                return prefixID + "0001";  // fixwidth default
            }
            //int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            int nextID = int.Parse(lastID) + 1;
            int lengthNumerID = lastID.Length;// - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;
        }

        public static string NextNVID(string lastID, string prefixID)
        {
            if (lastID == "")
            {
                return prefixID + "0001";  // fixwidth default
            }
            int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            //int nextID = int.Parse(lastID) + 1;
            int lengthNumerID = lastID.Length - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;
        }



        public static string NextVPID(string lastID, string prefixID)
        {
            if (lastID == "")
            {
                return prefixID + "001";  // fixwidth default
            }
            int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            int lengthNumerID = lastID.Length;// - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;
        }
        
        public static string NextNumID(string lastID, string prefixID)
        {
            if (lastID == "")
            {
                return prefixID + "0000001";  // fixwidth default
            }
            int nextID = int.Parse(lastID/*.Remove(0, prefixID.Length)*/) + 1;
            int lengthNumerID = lastID.Length;// - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;
        }

        public static string NextSoHD(string lastID, string prefixID)
        {
            if (lastID == "")
            {
                return prefixID + "0000001";  // fixwidth default
            }
            int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            //int nextID = int.Parse(lastID) + 1;
            int lengthNumerID = lastID.Length - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;
        }

    }
}