using System;
using System.Collections.Generic;
using System.Text;
using WaterUseDB.Resources;

namespace WaterUseAgent.Extensions
{
    public static class UpdateExtensions
    {
        public static IEnumerable<T> RemoveFCFIPCode<T>(this IEnumerable<T> fcList) where T : IFacilityCode
        {
            foreach (T fc in fcList)
            {
                yield return fc.RemoveFCFIPCode();
            }
        }
        public static T RemoveFCFIPCode<T>(this T fc) where T : IFacilityCode
        {
            fc.FacilityCode = fc.FacilityCode.Remove(0, 4);
            return fc;
        }

        public static IEnumerable<IFacilityCode> AddFCFIPCode(this IEnumerable<IFacilityCode> fcList, string FIPcode)
        {
            foreach (IFacilityCode fc in fcList)
            {
                yield return fc.AddFCFIPCode(FIPcode);
            }
        }     
        public static IFacilityCode AddFCFIPCode(this IFacilityCode fc, string FIPcode)
        {
            if (FIPcode.Length > 2)
                FIPcode = FIPcode.Substring(0, 2);
            
            fc.FacilityCode = fc.FacilityCode.Insert(0, "FC" + FIPcode);
            return fc;           
        }
    }
}
