public class ResultRecord 
{
    // 공통 영역
    public int tranningMode = 0;
    public string level = "";
    public string EndTime = "";    
    public string tranningCount = "";
    public string successCount = "";
    public string successRate = "";

    // 직선 훈련
    public string distance = "";
    public string height = "";

    // 거리 훈련
    public string distanceMethod = "";
    public string[] tranningCountForM = { "", "", "", "" };
    public string[] successCountForM = { "", "", "", "" };
    public string[] successRateForM = { "", "", "", "" };

    // 기울기 훈련
    public string condition = "";
    public string[] tranningCountForL = { "", "", "", "", "" };
    public string[] successCountForL = { "", "", "", "", "" };
    public string[] successRateForL = { "", "", "", "", "" };

    // 실전 훈련
    public string actualCupPoint = "";
    public string actualStartPoint = "";
    public string actualGradient = "";    

    public ResultRecord()
    {
        var option = GameOption.Instance;

        tranningMode = option.tranningMode;
        EndTime = System.DateTime.Now.ToString();

        switch (tranningMode)
        {
            case (int)TranningMode.STRAIGHT:
                var rule = EnumToData.Instance.StraightRule(option.straightLevel);
                string[] xy = rule.Split(",");

                level = option.straightLevel.ToString();
                distance = xy[0];
                if(option.straightLevel == 8)
                {
                    distance = "cup to hole";
                }
                height = xy[1];
                tranningCount = option.TranningCount.ToString();
                successCount = option.successCount.ToString();
                successRate = string.Format("{0:F1}", (100 / ((float)option.TranningCount / option.successCount)));

                // 0/0의 처리를 NaN으로 하기 때문에 예외처리 필요함
                if (successRate == "NaN")
                {
                    successRate = "0.0";
                }
                break;

            case (int)TranningMode.DISTANCE:
                level = EnumToData.Instance.DistanceLevelToKor(option.distanceLevel);
                distanceMethod = EnumToData.Instance.DistanceMethodToKor(option.distanceMethod);

                for (int i = 0; i < tranningCountForM.Length; i++)
                {
                    tranningCountForM[i] = option.tranningCountForM[i].ToString();
                }

                for (int i = 0; i < successCountForM.Length; i++)
                {
                    successCountForM[i] = option.successCountForM[i].ToString();
                }

                for (int i = 0; i < successRateForM.Length; i++)
                {
                    successRateForM[i] = string.Format("{0:F1}", (100 / ((float)option.tranningCountForM[i] / option.successCountForM[i])));
                }
                                
                for (int i = 0; i < successRateForM.Length; i++)
                {
                    if(successRateForM[i] == "NaN")
                    {
                        successRateForM[i] = "0.0";
                    }
                }
                break;

            case (int)TranningMode.GRADIENT:
                condition = EnumToData.Instance.GradientTitleToKor( option.gradientCondition).Substring(0,3);

                for (int i = 0; i < tranningCountForL.Length; i++)
                {
                    tranningCountForL[i] = option.tranningCountForL[i].ToString();
                }

                for (int i = 0; i < successCountForL.Length; i++)
                {
                    successCountForL[i] = option.successCountForL[i].ToString();
                }

                for (int i = 0; i < successRateForL.Length; i++)
                {
                    successRateForL[i] = string.Format("{0:F1}", (100 / ((float)option.tranningCountForL[i] / option.successCountForL[i])));
                }
                
                for (int i = 0; i < successRateForL.Length; i++)
                {
                    if (successRateForL[i] == "NaN")
                    {
                        successRateForL[i] = "0.0";
                    }
                }
                break;

            case (int)TranningMode.ACTUAL:
                actualCupPoint = EnumToData.Instance.ActualCupToKor(option.actualCupPoint);
                actualStartPoint = EnumToData.Instance.ActualStartToKor(option.actualStartPoint);
                actualGradient = EnumToData.Instance.ActualGradientToKor(option.actualGradient);
                tranningCount = option.TranningCount.ToString();
                successCount = option.successCount.ToString();
                successRate = string.Format("{0:F1}", (100 / ((float)option.TranningCount / option.successCount)));

                if (successRate == "NaN")
                {
                    successRate = "0.0";
                }
                break;
        }        
    }
}
