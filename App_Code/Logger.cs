using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace BTS.Util
{
    public class Logger
    {
        public const uint FATAL = 0;
        public const uint ERROR = 10;
        public const uint WARNING = 20;
        public const uint NOTICE = 30;
        public const uint INFO = 40;
        public const uint DETAILED = 50;
        public const uint DEBUG = 70;
        public const uint ALL = 100;

        protected Logger() { }

        public static string GetSeverityString(uint severity)
        {
            if (severity >= ALL) return "[ALL]";
            if (severity >= DEBUG) return "[DEBUG]";
            if (severity >= DETAILED) return "[DETAILED]";
            if (severity >= INFO) return "[INFO]";
            if (severity >= NOTICE) return "[NOTICE]";
            if (severity >= WARNING) return "[WARNING]";
            if (severity >= ERROR) return "[ERROR]";
            return "[FATAL]";
        }

        public static uint GetSeverityByString(string level)
        {
            if (level == null) return ALL;
            if (level.Equals("FATAL")) return FATAL;
            if (level.Equals("ERROR")) return ERROR;
            if (level.Equals("WARNING")) return ERROR;
            if (level.Equals("NOTICE")) return NOTICE;
            if (level.Equals("INFO")) return INFO;
            if (level.Equals("DETAILED")) return DETAILED;
            if (level.Equals("DEBUG")) return DEBUG;
            if (level.Equals("ALL")) return ALL;
            return ALL;
        }

        public static Logger GetLogger(uint number)
        {
            if (_table == null)
                return null;

            foreach (DictionaryEntry de in _table)
            {
                if ((uint)de.Key == number)
                    return (Logger)de.Value;
            }

            return null;
        }

        public void SetPath(string path)
        {
            _basepath = path;
        }

        public static Logger CreateLogger(uint number,string basepath, string filename, uint level)
        {
            return new Logger(number, basepath, filename, level, false);
        }

        public static Logger CreateLogger(uint number, string basepath, string filename, uint level, bool separateError)
        {
            return new Logger(number, basepath, filename, level, separateError);
        }

        // write severe + str
        public void Write(uint severity, string str)
        {
            if (_level < severity)
                return;

            OpenFile();
            _writer.Write(GetSeverityString(severity) + str);
            _writer.Flush();

            if ((severity == Logger.ERROR) && (_separateError))
            {
                _writer_err.Write(GetSeverityString(severity) + str);
                _writer_err.Flush();
            }
            CloseOpeningWriter();
        }
        // writeline severe + str
        public void WriteLine(uint severity, string str)
        {
            if (_level < severity)
                return;

            OpenFile();
            _writer.Write(GetSeverityString(severity));
            _writer.WriteLine(str);
            _writer.Flush();

            if ((severity == Logger.ERROR) && (_separateError))
            {
                _writer_err.Write(GetSeverityString(severity));
                _writer_err.WriteLine(str);
                _writer_err.Flush();
            }
            CloseOpeningWriter();
        }

        // writeline severe + time + str
        public void StampLine(uint severity, string str)
        {
            if (_level < severity)
                return;

            OpenFile();
            _writer.Write(GetSeverityString(severity) + " " + DateTime.Now + ": ");
            _writer.WriteLine(str);
            _writer.Flush();

            if ((severity == Logger.ERROR) && (_separateError))
            {
                _writer_err.Write(GetSeverityString(severity) + " " + DateTime.Now + ": ");
                _writer_err.WriteLine(str);
                _writer_err.Flush();
            }
            CloseOpeningWriter();
        }

        // write severe + time + str
        public void Stamp(uint severity, string str)
        {
            if (_level < severity)
                return;

            OpenFile();
            _writer.Write(GetSeverityString(severity) + " " + DateTime.Now + ": " + str);
            _writer.Flush();

            if ((severity == Logger.ERROR) && (_separateError))
            {
                _writer_err.Write(GetSeverityString(severity) + " " + DateTime.Now + ": " + str);
                _writer_err.Flush();
            }
            CloseOpeningWriter();
        }

        // writeline pure str
        public void PutLine(uint severity, string str)
        {
            if (_level < severity)
                return;

            OpenFile();
            _writer.WriteLine(str);
            _writer.Flush();

            if ((severity == Logger.ERROR) && (_separateError))
            {
                _writer_err.WriteLine(str);
                _writer_err.Flush();
            }
            CloseOpeningWriter();
        }


        protected void OpenFile()
        {
                if (((_keepOpening) && (!DateTime.Today.Equals(_currDate))) || (!_keepOpening))
                {
                    string[] fname = _filename.Split('.');
                    _currFile = fname[0] + "_" + StringUtil.Date2Filename(DateTime.Today) + "." + (fname.Length > 1 ? fname[1] : "");
                    _currFile_err = _currFile + ".ERROR";

                    CloseOpeningWriter();
                    if (Config.APPEND_LOG)
                    {
                        _writer = File.AppendText(_basepath + "\\" + _currFile);
                        if (_separateError)
                            _writer_err = File.AppendText(_basepath + "\\" + _currFile_err);
                    }
                    else
                    {
                        _writer = File.CreateText(_basepath + "\\" + _currFile);
                        if (_separateError)
                            _writer_err = File.CreateText(_basepath + "\\" + _currFile_err);
                    }
                    _currDate = DateTime.Today;
                }
        }

        protected void CloseFile()
        {
            if (!_keepOpening) CloseOpeningWriter();
        }

        private Logger(uint number,string basepath, string filename, uint level, bool separateError)
        {
            Logger logger = GetLogger(number);
            if (logger != null) logger.CloseOpeningWriter();

            _filename = filename;
            _basepath = basepath;
            string[] fname = filename.Split('.');
            _currFile = fname[0] + "_" + StringUtil.Date2Filename(DateTime.Today) + "." + (fname.Length > 1 ? fname[1] : "");
            _currFile_err = _currFile + ".ERROR";

            _separateError = separateError;

            CloseOpeningWriter();
            if (Config.APPEND_LOG)
            {                
                _writer = File.AppendText(_basepath + "\\" + _currFile);
                if (_separateError)
                    _writer_err = File.AppendText(_basepath + "\\" + _currFile_err);
            }
            else
            {
                _writer = File.CreateText(_basepath + "\\" + _currFile);
                if (_separateError)
                    _writer_err = File.CreateText(_basepath + "\\" + _currFile_err);
            }

            _level = level;

            if (_table == null)
                _table = new Hashtable();

            if (_table.ContainsKey(number)) _table.Remove(number);
            _table.Add(number, this);

            PrintHeader();
        }
        ~Logger()
        {
            //if (_writer != null) _writer.Close();
        }

        protected void CloseOpeningWriter()
        {
            if (_writer != null) _writer.Close();
            if (_writer_err != null) _writer_err.Close();
        }

        private void PrintHeader()
        {
            //_writer.WriteLine(((DateTime.Now).ToString()));
        }

        public bool SeparateError
        {
            get { return _separateError; }
            set { _separateError = value; }
        }

        public bool _keepOpening = false;

        private StreamWriter _writer;
        private StreamWriter _writer_err;
        private string _filename;
        private string _basepath;
        private string _currFile;
        private string _currFile_err;
        private DateTime _currDate;
        private static Hashtable _table;
        private uint _level;
        private bool _separateError = false;
    }
}
