using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class IniManager : MonoSingleton<IniManager>
{
    protected Dictionary<string, Dictionary<string, Dictionary<string, string>>> iniFileDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

    /// <summary>
    /// 初始化各类资源配置文件
    /// </summary>
    public override void Init()
    {
        base.Init();

        Debug.Log("初始化配置文件", gameObject);
        /*
        ReadIni(InIInfoCommon.ResourceConfigFileName);
        ReadIni(InIInfoCommon.PrefabConfigFileName);
        ReadIni(InIInfoCommon.AttributeConfigFileName);
        */
    }

    /// <summary>
    /// 读取InI文件具体方法
    /// </summary>
    protected void ReadIni(string pConfigFileName)
    {
        string configPath = Application.dataPath + "/StreamingAssets/" + pConfigFileName + ".ini";
        string[] tempTitles = InIRead.INIGetAllSectionNames(configPath);

        if (tempTitles.Length == 0)
        {
            throw new ArgumentException("pConfigFileName is no exist");
        }
        Dictionary<string, Dictionary<string, string>> tempSumDictionary = new Dictionary<string, Dictionary<string, string>>();
        foreach (var itemTitle in tempTitles)
        {
            string[] tempKeys = InIRead.INIGetAllItemKeys(itemTitle, configPath);
            Dictionary<string, string> tempKeyDictionary = new Dictionary<string, string>();
            foreach (var itemKey in tempKeys)
            {
                tempKeyDictionary[itemKey] = InIRead.INIGetStringValue(itemTitle, itemKey, "", configPath);
            }
            tempSumDictionary[itemTitle] = tempKeyDictionary;
        }
        iniFileDictionary[pConfigFileName] = tempSumDictionary;
    }

    /**************************************** Ini字典操作 ************************************************/
    public string[] GetTitles(string pConfigFileName)
    {
        if (!iniFileDictionary.ContainsKey(pConfigFileName))
        {
            return null;
        }

        return iniFileDictionary[pConfigFileName].Keys.ToArray();
    }
    public string[] GetKeys(string pConfigFileName, string pTitle)
    {
        if (!iniFileDictionary.ContainsKey(pConfigFileName) || !iniFileDictionary[pConfigFileName].ContainsKey(pTitle))
        {
            return null;
        }

        return iniFileDictionary[pConfigFileName][pTitle].Keys.ToArray();
    }
    public string GetValue(string pConfigFileName, string pTitle, string pKey)
    {
        if (!iniFileDictionary.ContainsKey(pConfigFileName) || !iniFileDictionary[pConfigFileName].ContainsKey(pTitle) ||
            !iniFileDictionary[pConfigFileName][pTitle].ContainsKey(pKey))
        {
            return null;
        }

        return iniFileDictionary[pConfigFileName][pTitle][pKey];
    }
    public Dictionary<string, Dictionary<string, string>> GetInIDictionary(string pConfigFileName)
    {
        if (!iniFileDictionary.ContainsKey(pConfigFileName))
        {
            return null;
        }

        return iniFileDictionary[pConfigFileName];
    }
    /****************************************************************************************************/
}
