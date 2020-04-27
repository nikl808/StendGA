using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace stend
{
    abstract class FileWriter
    {
        protected QuestionMessage _question = new QuestionMessage();
        
        public void WriteFile<T>(string filenamePath, T obj)
        {
            CheckDir(filenamePath);
            Write<T>(filenamePath, obj);
        }

        protected virtual void CheckDir(string filenamePath)
        {
            string path = filenamePath.Substring(0, filenamePath.LastIndexOf('\\'));
            if (!Directory.Exists(path)) 
            {
                DirectoryInfo dir = new DirectoryInfo(@path);
                dir.Create();
            }
        }

        protected abstract void Write<T>(string filenamePath, T obj);
        protected delegate bool AskOperation(string caption, string text);
    }

    class XMLFileWriter : FileWriter
    {
        protected override void Write<T>(string filenamePath, T obj)
        { 
            XmlSerializer formatter = new XmlSerializer(typeof (T));
            FileStream fs = null;

            if (File.Exists(filenamePath))
            {
                AskOperation answer = _question.Message;
                if (answer("Warning", filenamePath + " is exist. Do you want overwrite it?"))
                {
                    try
                    {
                        fs = new FileStream(@filenamePath, FileMode.Open);
                        formatter.Serialize(fs, obj);
                    }
                    catch (Exception ex)
                    {
                        Error.instance.HandleExceptionMessage(ex);
                    }
                    finally
                    {
                        if(fs!=null)fs.Close();
                    }
                }
            }
            else    
            {
                try
                {
                     fs = new FileStream(@filenamePath, FileMode.Create);
                     formatter.Serialize(fs, obj);
                }
                catch (Exception ex)
                {
                    Error.instance.HandleExceptionMessage(ex);
                }
                finally
                {
                    if(fs!=null)fs.Close();
                }
            }
        }
    }

    class ConfigFileWriter : FileWriter
    {
        protected override void Write<T>(string filenamePath, T obj)
        {
            FileInfo file = new FileInfo(@filenamePath);
            StreamWriter txt_writer = null;

            if (!File.Exists(filenamePath))
            {
                txt_writer = file.CreateText();
                txt_writer.WriteLine(obj.ToString());
                txt_writer.Close();
            }

            else
            {
                GenericFileReader reader = new GenericFileReader();
                //get file
                string fileread = reader.ReadFile<string>(filenamePath);
               
                //find replaceable parameter
                string find = obj.ToString().Substring(obj.ToString().IndexOf('{'), obj.ToString().IndexOf('}') + 1);

                //replace line
                if (fileread.Contains(find))
                {
                    //get replaceable substring
                    int FirstIndex = fileread.IndexOf(find);
                    int lastIndex = fileread.IndexOf(';', FirstIndex) + 1;
                    string replSubstr = fileread.Substring(FirstIndex, lastIndex - FirstIndex);

                    if (!replSubstr.Equals(obj.ToString()))
                    {
                        try
                        {
                            txt_writer = file.CreateText();
                            txt_writer.Write(fileread.Replace(replSubstr, obj.ToString()));
                        }
                        catch (Exception ex)
                        {
                            Error.instance.HandleExceptionMessage(ex);
                        }
                        finally
                        {
                            if (txt_writer != null) txt_writer.Close();
                        }
                    }
                }

                //add new line
                else
                {
                    try
                    {
                        txt_writer = file.AppendText();
                        txt_writer.WriteLine(obj.ToString());

                    }
                    catch (Exception ex)
                    {
                        Error.instance.HandleExceptionMessage(ex);
                    }
                    finally
                    {
                        if (txt_writer != null) txt_writer.Close();
                    }
                }
            }
        }
    }
}