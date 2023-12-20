using System;
using System.Xml.Serialization;

/// <summary>
/// Class : RuleFile
/// Desc  : 게임 모드에 대한 룰을 규정
/// Date  : 2022-08-17
/// Autor : Kang Cheol Woong

[Flags] // 여러 개의 선택을 하나의 값으로 조합할 수 있다.
public enum SelectModeControl
{
    NONE            = 0,
    ONEOUT          = 1 << 0,
    FULLCOUNT       = 1 << 1,
    AI              = 1 << 2,
    HOMERUNDERBY    = 1 << 3
}

public enum SelectModeIndex
{
    ONEOUT = 0,
    FULLCOUNT,
    AI,
    HOMERUNDERBY
}

public class RuleFile
{    
    public string notice = "활성화를 원하는 Mode를 |(OR)를 기준으로 입력하세요.";

    // game mode default값
    public string str = "ONEOUT|FULLCOUNT|AI|HOMERUNDERBY";

    [XmlIgnore] 
    public SelectModeControl mode;

    public SelectModeControl SetMode()
    {
        // 기본 모드를 0으로 초기화
        mode = SelectModeControl.NONE;

        // 문자열을 (|) 기준으로 나눠서 보관
        // ""가 아닌 ''로 작동함. (이전 버전 이슈)
        var temps = str.Split('|');

        foreach (var item in temps)
        {
            var e = (SelectModeControl)Enum.Parse(typeof(SelectModeControl), item);
            mode |= e;
        }
        return mode;
    }
}
