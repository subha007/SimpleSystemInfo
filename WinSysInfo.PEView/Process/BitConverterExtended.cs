﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// CLass extends the functionalitie of <see cref="BitConverter"/>
    /// </summary>
    public static class BitConverterExtended
    {
        /// <summary>
        /// Converts Decimal value to byte array
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static byte[] GetBytes(decimal dec)
        {
            //Load four 32 bit integers from the Decimal.GetBits function
            Int32[] bits = decimal.GetBits(dec);

            //Create a temporary list to hold the bytes
            List<byte> bytes = new List<byte>();

            //iterate each 32 bit integer
            foreach(Int32 i in bits)
            {
                //add the bytes of the current 32bit integer
                //to the bytes list
                bytes.AddRange(BitConverter.GetBytes(i));
            }
            //return the bytes list as an array
            return bytes.ToArray();
        }

        /// <summary>
        /// Converts byte array to decimal
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static decimal ToDecimal(byte[] bytes)
        {
            //check that it is even possible to convert the array
            if(bytes.Count() != 16)
                throw new Exception("A decimal must be created from exactly 16 bytes");

            //make an array to convert back to int32's
            Int32[] bits = new Int32[4];
            for(int i = 0; i <= 15; i += 4)
            {
                //convert every 4 bytes into an int32
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }

            //Use the decimal's new constructor to
            //create an instance of decimal
            return new decimal(bits);
        }
    }
}
