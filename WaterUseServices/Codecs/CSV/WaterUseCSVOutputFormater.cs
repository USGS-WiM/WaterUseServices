using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using WiM.Codecs.CSV;
using WaterUseAgent.Resources;

namespace WaterUseServices.Codecs.CSV
{
    public class WaterUseCSVOutputFormater: CSVOutputFormatter
    {
        protected override bool CanWriteType(Type type)
        {

            if (type == null)
                throw new ArgumentNullException("type");

            return type == typeof(Dictionary<string, Wateruse>);
        }

        public WaterUseCSVOutputFormater():base()
        { }
        protected override void writeStream(Type type, object value, Stream stream)
        {
            StringWriter sWriter = null;
            try
            {

                sWriter = this.getSourceWateruseCSV((IDictionary<string, Wateruse>)value);              
                
                var streamWriter = new StreamWriter(stream);
                streamWriter.Write(sWriter.ToString());
                streamWriter.Flush();
            }
            catch (Exception ex)
            {
                var x = ex;
                throw;
            }
        }

        private StringWriter getSourceWateruseCSV(IDictionary<string, Wateruse> value) {
            Int16 stachObj =0;
            try
            {
                //Type itemType = type.GetGenericArguments()[0];

                StringWriter stringWriter = new StringWriter();


                stringWriter.WriteLine(string.Join<string>(",", new string[] { "Time", value.FirstOrDefault().Value.ProcessDate.ToString() }));
                stringWriter.WriteLine(string.Join<string>(",", new string[] { "StartYear", value.FirstOrDefault().Value.StartYear.ToString() }));
                stringWriter.WriteLine(string.Join<string>(",", new string[] { "EndYear", value.FirstOrDefault().Value.EndYear.HasValue? value.FirstOrDefault().Value.EndYear.Value.ToString(): value.FirstOrDefault().Value.StartYear.ToString() }));
                stringWriter.WriteLine(string.Join<string>(",", new string[] { "Units", "Million Gallons per Day" }));
                stringWriter.WriteLine();//blank line

                //headers
                var hr= value.Where(w =>
                          w.Value.Return != null).SelectMany(wu =>
                         wu.Value.Return.Annual.Select(Ann => "ANN_Ret_" + Ann.Key)).Distinct().Concat(

                value.Where(w =>
                             w.Value.Return != null).SelectMany(wu => wu.Value.Return.Monthly.SelectMany(mon =>
                             mon.Value.Month.Select(v => mon.Key.ToString("00") + "_Ret_" + v.Key))).Distinct().OrderBy(s => s));


                var hw = value.Where(w =>
                             w.Value.Withdrawal != null).SelectMany(wu =>
                            wu.Value.Withdrawal.Annual.Select(Ann => "ANN_With_" + Ann.Key)).Distinct().Concat(

                value.Where(w =>
                             w.Value.Withdrawal != null).SelectMany(wu => wu.Value.Withdrawal.Monthly.SelectMany(mon =>
                             mon.Value.Month.Select(v => mon.Key.ToString("00") + "_With_" + v.Key))).Distinct().OrderBy(s => s));
                //catagories should not be output.--> This is not quite right
                //value.Where(w =>
                //             w.Value.Withdrawal != null).Where(wu=>wu.Value.Withdrawal.Monthly.Any(m=>m.Value.Code != null)).SelectMany(wu => 
                //                wu.Value.Withdrawal.Monthly.SelectMany(mon =>mon.Value.Code)).Select(cd=>cd.Key).Distinct());

                var headers = hr.Concat(hw).ToList();
               
                headers.Insert(0, "FacilityCode");
                stringWriter.WriteLine(String.Join(",", headers));

                //stringWriter.WriteLine(string.Join<string>(",", itemType.GetProperties().Select(x => x.Name)));

                foreach (var obj in (IDictionary<string, WaterUseAgent.Resources.Wateruse>)value)
                {
                    string[] line = new string[headers.Count];
                    line[0]=(obj.Key);

                    if (obj.Value.Return != null) {
                        obj.Value.Return.Annual.ToList().ForEach(ann=> line[headers.IndexOf("ANN_Ret_"+ann.Key)]= ann.Value.Value.ToString("N4"));

                        obj.Value.Return.Monthly.Where(c => c.Value.Month != null).SelectMany(m => m.Value.Month.Select(wu => 
                            new { key = m.Key.ToString("00") + "_Ret_" + wu.Key, value = wu.Value.Value }))
                            .ToList().ForEach(item => line[headers.IndexOf(item.key)] = item.value.ToString("N4"));
                    }
                    if (obj.Value.Withdrawal != null)
                    {
                        obj.Value.Withdrawal.Annual.ToList().ForEach(ann => line[headers.IndexOf("ANN_With_" + ann.Key)] = ann.Value.Value.ToString("N4"));

                        obj.Value.Withdrawal.Monthly.Where(c => c.Value.Month != null).SelectMany(m => m.Value.Month.Select(wu => new { key = m.Key.ToString("00") + "_With_" + wu.Key, value = wu.Value.Value }))
                            .ToList().ForEach(item => line[headers.IndexOf(item.key)] = item.value.ToString("N4"));

                        //catagories should not be output --> this is not quite right
                        //obj.Value.Withdrawal.Monthly.Where(c=>c.Value.Code != null).SelectMany(m => m.Value.Code.Select(wu => new { key = m.Key.ToString("00") + "_With_" + wu.Key, value = wu.Value.Value }))
                        //    .ToList().ForEach(item => line[headers.IndexOf(item.key)] = item.value.ToString("N4"));
                    }

                    stringWriter.WriteLine(string.Join(",",line));
                    stachObj++;

                }//next obj

                return stringWriter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        

        private string join(string str, string _val) {
            if (!string.IsNullOrEmpty(_val))
            {

                //Check if the value contans a comma and place it in quotes if so
                if (_val.Contains(","))
                    _val = string.Concat("\"", _val, "\"");

                //Replace any \r or \n special characters from a new line with a space
                if (_val.Contains("\r"))
                    _val = _val.Replace("\r", " ");
                if (_val.Contains("\n"))
                    _val = _val.Replace("\n", " ");

                return string.Concat(str, _val, ",");
            }
            else
            {

                return string.Concat(str, string.Empty, ",");
            }//end if
        }
        
    }   
}
