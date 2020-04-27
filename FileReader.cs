using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

namespace stend
{
    abstract class FileReader
    {
        public T ReadFile<T>(string filenamePath) { return Read<T>(filenamePath); }

        protected abstract T Read<T>(string filenamePath);
    }

    class XMLFileReader : FileReader
    {
        protected override T Read<T>(string filenamePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(@filenamePath, FileMode.Open);
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                return (T)deserializer.Deserialize(fs);
                
            }
            catch (Exception ex)
            {
               Error.instance.HandleExceptionMessage(ex);
                return default(T);
            }
            finally
            {
                if(fs!=null)fs.Close();
            }
        }   
    }

    class GenericFileReader : FileReader
    {
        protected override T Read<T>(string filenamePath) 
        {
            FileInfo file = new FileInfo(@filenamePath); 
            StreamReader txt_reader = null;
            try
            {
                txt_reader = file.OpenText();
                return (T)Convert.ChangeType(txt_reader.ReadToEnd(), typeof(T),new CultureInfo("en-US"));
            }
            catch (Exception ex)
            {
                Error.instance.HandleExceptionMessage(ex);
                return default(T);
            }
            finally
            {
                if (txt_reader != null) txt_reader.Close();
            }
        }   
    }
}
