using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using Excel;
using System.Data;

public class ExportExcelDB
{
    [MenuItem("DataTool/GenerateSQL")]
    public static void GenerateSQL()
    {
        FileInfo info;
        FileStream stream;
        IExcelDataReader excelReader;
        DataSet result;

        string[] files = Directory.GetFiles(Application.dataPath + "/Design/Excels", "*.xlsx", SearchOption.TopDirectoryOnly);

        try
        {
            string code1 = "";
            //string code2 = "";

            foreach (string path in files)
            {
                info = new FileInfo(path);
                stream = info.Open(FileMode.Open, FileAccess.Read);
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                result = excelReader.AsDataSet();

                int rowCount = result.Tables[0].Rows.Count;
                int colCount = result.Tables[0].Columns.Count;

                string className = info.Name.Substring(0, info.Name.Length - 5);    //去掉.xlsx

                code1 += "drop table " + className + ";\n" ;
                code1 += "create table " + className + "(";

                for (int i = 0; i < colCount; i++) {
                    string name = result.Tables[0].Rows[0][i].ToString();

                    if (name == "id")
                    {
                        code1 += name + " integer PRIMARY KEY NOT NULL";
                    }
                    else
                    {
                        code1 += name;
                    }

                    if (i == colCount - 1)
                    {
                        code1 += ");\n";
                    }
                    else {
                        code1 += ",";
                    }
                }

                for (int i = 3; i < rowCount; i ++ )
                {
                    code1 += "insert into " + className + " values (";

                    for (int j = 0; j < colCount; j++) {

                        string name = result.Tables[0].Rows[i][j].ToString();
                        string type = result.Tables[0].Rows[1][j].ToString();

                        if (type.Equals("string"))
                        {
                            code1 += "\'" + name + "\'";
                        }
                        else {
                            code1 += name;
                        }

                        if (j == colCount - 1 && i == rowCount - 1) {
                            code1 += ");";
                        }
                        else if (j == colCount - 1)
                        {
                            code1 += ");\n";
                        }
                        else
                        {
                            code1 += ",";
                        }
                    }
                }

                excelReader.Close();
                stream.Close();
            }

            WriteClass(Application.dataPath + "/Design/Excels/DB.sql", code1);
        }
        catch (IndexOutOfRangeException exp)
        {
            Debug.LogError(exp.StackTrace);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    private static void WriteClass(string path, string code)
    {
        var utf8WithoutBom = new System.Text.UTF8Encoding(false);

        System.IO.File.WriteAllText(path, code, utf8WithoutBom);
    }

    [MenuItem("DataTool/GenerateDB")]
    public static void GenerateDB() {
        System.IO.DirectoryInfo parent = System.IO.Directory.GetParent(Application.dataPath);
        string projectPath = parent.ToString();
        
        string para = "";

        string para1 = Application.streamingAssetsPath + "/DB/DB.db";//"F:/workspace/MahjongMaster/Assets/StreamingAssets/DB/DB.db";
        string para2 = Application.dataPath + "/Design/Excels/DB.sql";//"F:/workspace/MahjongMaster/Assets/Design/Excels/DB.sql";

        string cmd = projectPath + "/Tools/SQLite/Test/GenerateDB.bat";

        string para3 = projectPath + "/Tools/SQLite/sqlite-tools-win32-x86-3180000/sqlite3";

        para = para1 + " " + para2 + " " + para3;

        //Process.ProcessCommand("F:\\workspace\\MahjongMaster\\Tools\\SQLite\\sqlite-tools-win32-x86-3180000\\sqlite3.exe", para);

        Process.ProcessCommand(cmd, para);
    }
}
