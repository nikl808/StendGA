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
        public string[] ReadFile(string filenamePath, int numLines) { return Read(filenamePath, numLines); }

        protected abstract T Read<T>(string filenamePath);
        protected abstract string[] Read(string filenamePath, int numLines);
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

        protected override string[] Read(string filenamePath, int numLines)
        {
            throw new NotImplementedException();
        }
    }

    class GenericFileReader : FileReader
    {
        protected override string[] Read(string filenamePath, int numLines) 
        {
            FileInfo file = new FileInfo(@filenamePath); 
            StreamReader txt_reader = null;
            try
            {
                txt_reader = file.OpenText();
                string[] fStr = new string[numLines];
                int i = 0;
                while ((fStr[i] = txt_reader.ReadLine()) != null) { i++; } 

                return fStr;
            }
            catch (Exception ex)
            {
                Error.instance.HandleExceptionMessage(ex);
                return new string[0];
            }
            finally
            {
                if (txt_reader != null) txt_reader.Close();
            }
        }
        protected override T Read<T>(string filenamePath)
        {
            throw new NotImplementedException();
        }
    }
}
