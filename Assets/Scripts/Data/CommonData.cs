using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CARD_STATUS
{
    INVAILD,    //不能点击
    VAILD,      //能点击
    SELECTED,   //选中
    UNSELECTED, //未选中
    DONE        //成功消除
}

public enum FIGURE_TYPE
{
    captain,
    mechanic,
    doctor,
    mate
}

public enum DEFAULT_NPC
{
    Sandy,
    Tom,
    Joy,
    userdefault
}

public enum LEVEL_TYPE
{
    level,
    cave
}

public enum CARD_SHOW_TYPE
{
    TEST1,
    TEST2,
    TEST3,
    TEST4
}

public enum CARD_REFRESH_TYPE
{
    TEST2,
    TEST4
}

public enum CARD_TYPE_SEASON
{
    chun,
    xia,
    qiu,
    dong
}

public enum CARD_TYPE_PLANT
{
    lan,
    ju,
    mei,
    zhu
}

public enum CARD_TYPE
{
    //none,

    yibing = 0,
    erbing,
    sanbing,
    sibing,
    wubing,
    liubing,
    qibing,
    babing,
    jiubing,

    beifeng,
    xifeng,
    nanfeng,
    dongfeng,
    zhong,
    fa,

    yiwan,
    erwan,
    sanwan,
    siwan,
    wuwan,
    liuwan,
    qiwan,
    bawan,
    jiuwan,

    yitiao,
    ertiao,
    santiao,
    sitiao,
    wutiao,
    liutiao,
    qitiao,
    batiao,
    jiutiao,

    baipi,

    plant,
    season,

    max

    //tile_bg
}

public class CommonData
{
    public const string MAHJONG_TILE_SET = "Texture/Atlas/smooth";
    public const string FIGURE_PATH = "Texture/figure/";
    // 当前最高游戏关卡
    public const string BEST_LEVEL = "BestLevel";
    // 体力值
    public const string POWER = "Heart";
    // 目标时间戳
    public const string AIM_TIME = "AimTime";
    public static float deltaTime = -1;

    public static List<string> seasonList;
    public static List<string> plantList;
    // 当前所在关卡
    public static int currentLV;
    public static int caveLV;
    public static LEVEL_TYPE currentLvType = LEVEL_TYPE.level;
    public const int HEART_MAX = 5;
    public const int BASE_WIDTH = 640;
    // 体力恢复时间
    public const int HEART_COOLING_TIME = 30;

    //是否闯关成功
    public static bool isSuccessLv = false;
}

