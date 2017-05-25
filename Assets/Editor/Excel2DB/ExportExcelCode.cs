using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using Excel;
using System.Data;

public class ExportExcelCode
{
    [MenuItem("DataTool/GenerateCode")]
    public static void GenerateCode()
    {
        FileInfo info;
        FileStream stream;
        IExcelDataReader excelReader;
        DataSet result;

        string[] files = Directory.GetFiles(Application.dataPath + "/Design/Excels", "*.xlsx", SearchOption.TopDirectoryOnly);

        try
        {
            string code1;
            //string code2;

            foreach (string path in files)
            {
                info = new FileInfo(path);
                stream = info.Open(FileMode.Open, FileAccess.Read);
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                result = excelReader.AsDataSet();
                
                int rowCount = result.Tables[0].Rows.Count;
                int colCount = result.Tables[0].Columns.Count;

                string className = info.Name.Substring(0, info.Name.Length - 5);    //去掉.xlsx

                code1 = "";
                //code2 = "";
                code1 += "using System;\n";
                code1 += "using System.Collections;\n";
                code1 += "using System.Collections.Generic;\n";
                code1 += "using System.IO;\n";
                code1 += "using UnityEngine;\n";
                code1 += "using SQLite4Unity3d;\n\n";

                code1 += "public class " + className + "\n";
                code1 += "{\n";

                for (int col = 0; col < colCount; col++)
                {
                    string name = result.Tables[0].Rows[0][col].ToString();
                    string type = result.Tables[0].Rows[1][col].ToString();

                    if (name == "id") {
                        code1 += "    [PrimaryKey, AutoIncrement]\n";
                    }

                    code1 += "    public " + type + " " + name + " { get; set; }\n";



                    //if (type.Equals("int"))
                    //{
                    //    code2 += "                name." + name + " = " + "br.ReadInt32();\n";
                    //}
                    //else if (type.Equals("string"))
                    //{
                    //    code2 += "                name." + name + " = " + "br.ReadString();\n";
                    //}
                    //else if (type.Equals("double"))
                    //{
                    //    code2 += "                name." + name + " = " + "br.ReadDouble();\n";
                    //}

                }
                code1 += "\n    public " + className + "()\n";
                code1 += "    {}\n";

                code1 += "}\n";

                WriteClass(Application.dataPath + "/Scripts/Data/Excel2CS/" + className + ".cs", className, code1);



                excelReader.Close();
                stream.Close();
            }
        }
        catch (IndexOutOfRangeException exp)
        {
            Debug.LogError(exp.StackTrace);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    private static void WriteClass(string path, string className, string code)
    {
        System.IO.File.WriteAllText(path, code, System.Text.UnicodeEncoding.UTF8);
    }
}
