using System;
using System.Collections.Generic;
using System.Linq;

class LevelTalks
{
    private int talkIndex;
    public List<TalkBorder> talklist;

    public LevelTalks()
    {
        talklist = new List<TalkBorder>();

        DataBaseService service = DataBaseService.GetInstance();
        var talkborder = service.GetTalkBorder(CommonData.currentLV);
        talklist = talkborder.ToList<TalkBorder>();
    }

    public TalkBorder getCurrentTalkBorder() {
        TalkBorder talkborder = null;
        if (talklist != null && talklist.Count > talkIndex)
        {
            talkborder = talklist[talkIndex];
            talkIndex++;
        }
        return talkborder;
    }

}
