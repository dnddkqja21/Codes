public class EnumToData
{
    private static EnumToData instance = null;
    public static EnumToData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnumToData();
            }
            return instance;
        }
    }

    // 트레이닝 모드 한글로 리턴
    public string TranningModeToKor(int mode)
    {
        string kor = "";
        switch (mode)
        {
            case (int)TranningMode.STRAIGHT:
                kor = "직선 훈련";
                break;
            case (int)TranningMode.DISTANCE:
                kor = "거리 훈련";
                break;
            case (int)TranningMode.GRADIENT:
                kor = "기울기 훈련";
                break;
            case (int)TranningMode.ACTUAL:
                kor = "실전 훈련";
                break;
        }
        return kor;
    }

    // 직선 훈련 단계 별 제목
    public string StraightTitleToKor(int level)
    {
        string kor = "";
        switch(level)
        {
            case (int)StraightLevel.ONE:
                kor = "직선 가이드 라인을 따라 퍼팅을 훈련하세요.";
                break;
            case (int)StraightLevel.TWO:                
            case (int)StraightLevel.THREE:                
            case (int)StraightLevel.FOUR:
                kor = "긴 직선 가이드 라인을 따라 퍼팅을 훈련하세요.";
                break;
            case (int)StraightLevel.FIVE:                
            case (int)StraightLevel.SIX:                
            case (int)StraightLevel.SEVEN:
                kor = "얇고 긴 직선 가이드 라인을 따라 퍼팅을 훈련하세요.";
                break;
            case (int)StraightLevel.EIGHT:                
                kor = "정해진 컵으로 퍼팅을 훈련하세요.";
                break;
        }
        return kor;
    }
    
    // 직선 훈련 단계 별 내용
    public string StraightInfoToKor(int level)
    {
        string kor = "";
        switch (level)
        {
            case (int)StraightLevel.ONE:
            case (int)StraightLevel.TWO:
            case (int)StraightLevel.THREE:
            case (int)StraightLevel.FOUR:
            case (int)StraightLevel.FIVE:
            case (int)StraightLevel.SIX:
            case (int)StraightLevel.SEVEN:
                kor = "직선 가이드 라인의 범위를 벗어나지 않도록" +
                                "\n퍼팅을 진행해야 합니다.";
                break;

            case (int)StraightLevel.EIGHT:
                kor = "직선 가이드 라인 범위를 벗어나지 않고," +
                                "\n표시된 컵에 볼이 들어가도록 퍼팅을 진행해야 합니다.";
                break;
        }
        return kor;
    }

    // 직선 훈련 규칙
    public string StraightRule(int level)
    {
        // 임시로 지정 (가로, 세로 길이 (단위 cm))
        string rule = "x,y";

        switch(level)
        {
            case (int)StraightLevel.ONE:
                rule = "60,15";
                break;
            case (int)StraightLevel.TWO:
                rule = "100,15";
                break;
            case (int)StraightLevel.THREE:
                rule = "150,15";
                break;
            case (int)StraightLevel.FOUR:
                rule = "200,15";
                break;
            case (int)StraightLevel.FIVE:
                rule = "250,10";
                break;
            case (int)StraightLevel.SIX:
                rule = "300,10";
                break;
            case (int)StraightLevel.SEVEN:
                rule = "350,10";
                break;
            case (int)StraightLevel.EIGHT:
                //rule = "start point to cup";
                rule = "350,10";
                break;
        }
        return rule;
    }

    // 거리 훈련 난이도 한글로 리턴
    public string DistanceLevelToKor(int mode)
    {
        string kor = "";
        switch (mode)
        {
            case (int)DistanceLevel.EASY:
                kor = "쉬움";
                break;
            case (int)DistanceLevel.NORMAL:
                kor = "보통";
                break;
            case (int)DistanceLevel.HARD:
                kor = "어려움";
                break;            
        }
        return kor;
    }

    // 거리 훈련 방식 한글로 리턴
    public string DistanceMethodToKor(int method)
    {
        string kor = "";
        switch(method)
        {
            case (int)DistanceMethod.FIXED:
                kor = "고정";
                break;
            case (int)DistanceMethod.ROTATION:
                kor = "순환";
                break;
            case (int)DistanceMethod.RANDOM:
                kor = "랜덤";
                break;
        }
        return kor;
    }

    // 거리 훈련 방식에 따른 훈련 내용
    public string DistanceInfoToKor(int method)
    {
        string kor = "";
        switch (method)
        {
            case (int)DistanceMethod.FIXED:
                kor = "수동으로 목표를 지정해서 훈련합니다.";
                break;
            case (int)DistanceMethod.ROTATION:
                kor = "성공 시 목표 영역이 순차적으로 변경됩니다.";
                break;
            case (int)DistanceMethod.RANDOM:
                kor = "성공 시 목표 영역이 랜덤하게 변경됩니다.";
                break;
        }
        return kor;
    }

    // 거리 훈련 규칙
    public float DistanceRule(float point)
    {
        float distance = 0;

        switch(point)
        {
            case 1:
                distance = 50;
                break;
            case 2:
                distance = 70;
                break;
            case 3:
                distance = 90;
                break;
            case 4:
                distance = 110;
                break;
            case 5:
                distance = 130;
                break;
            case 6:
                distance = 150;
                break;
            case 7:
                distance = 170;
                break;
            case 8:
                distance = 190;
                break;
            case 9:
                distance = 210;
                break;
            case 10:
                distance = 230;
                break;
            case 11:
                distance = 250;
                break;
            case 12:
                distance = 270;
                break;
            case 13:
                distance = 290;
                break;
            case 14:
                distance = 310;
                break;
            case 15:
                distance = 330;
                break;
            case 16:
                distance = 350;
                break;
        }
        return distance;
    }

    // 기울기 훈련 환경 별 제목
    public string GradientTitleToKor(int condition)
    {
        string kor = "";
        switch(condition)
        {
            case (int)GradientCondition.ONE:
                kor = "1단계 : 오르막 기울기를 훈련합니다.";
                break;
            case (int)GradientCondition.TWO:
                kor = "2단계 : 좌측에서 우측으로 내려오는 기울기를 훈련합니다.";
                break;
            case (int)GradientCondition.THREE:
                kor = "3단계 : 우측에서 좌측으로 내려오는 기울기를 훈련합니다.";
                break;
            case (int)GradientCondition.FOUR:
                kor = "4단계 : 내리막 기울기를 훈련합니다.";
                break;
            case (int)GradientCondition.FIVE:
                kor = "5단계 : 오르막, 우측 기울기를 함께 훈련합니다.";
                break;
            case (int)GradientCondition.SIX:
                kor = "6단계 : 오르막, 좌측 기울기를 함께 훈련합니다.";
                break;
            case (int)GradientCondition.SEVEN:
                kor = "7단계 : 내리막, 우측 기울기를 함께 훈련합니다.";
                break;
            case (int)GradientCondition.EIGHT:
                kor = "8단계 : 내리막, 좌측 기울기를 함께 훈련합니다.";
                break;
        }
        return kor;
    }

    // 기울기 훈련 환경 별 설명 문구
    public string GradientEachInfoToKor(int condition)
    {
        string kor = "";
        switch (condition)
        {
            case (int)GradientCondition.ONE:
                kor = "컵 방향 오르막";
                break;
            case (int)GradientCondition.TWO:
                kor = "우측 방향 내리막";
                break;
            case (int)GradientCondition.THREE:
                kor = "좌측 방향 내리막";
                break;
            case (int)GradientCondition.FOUR:
                kor = "컵 방향 내리막";
                break;
            case (int)GradientCondition.FIVE:
                kor = "컵 오르막, 우측 내리막";
                break;
            case (int)GradientCondition.SIX:
                kor = "컵 오르막, 좌측 내리막";
                break;
            case (int)GradientCondition.SEVEN:
                kor = "컵 내리막, 우측 내리막";
                break;
            case (int)GradientCondition.EIGHT:
                kor = "컵 내리막, 좌측 내리막";
                break;
        }
        return kor;
    }

    // 실전 훈련 컵 포인트
    public string ActualCupToKor(int method)
    {
        string kor = "";
        switch(method)
        {
            case (int)ActualCupPoint.FIXED:
                kor = "고정";
                break;
            case (int)ActualCupPoint.RANDOM:
                kor = "랜덤";
                break;
        }
        return kor;
    }

    // 실전 훈련 스타트 포인트
    public string ActualStartToKor(int method)
    {
        string kor = "";
        switch (method)
        {
            case (int)ActualStartPoint.FIXED:
                kor = "고정";
                break;
            case (int)ActualStartPoint.RANDOM:
                kor = "랜덤";
                break;
        }
        return kor;
    }

    // 실전 훈련 기울기
    public string ActualGradientToKor(int level)
    {
        string kor = "";
        switch (level)
        {
            case (int)ActualGradient.NONE:
                kor = "없음";
                break;
            case (int)ActualGradient.ONE:
                kor = "1단계";
                break;
            case (int)ActualGradient.TWO:
                kor = "2단계";
                break;
            case (int)ActualGradient.THREE:
                kor = "3단계";
                break;
            case (int)ActualGradient.FOUR:
                kor = "4단계";
                break;
            case (int)ActualGradient.FIVE:
                kor = "5단계";
                break;
        }
        return kor;
    }
}


// 메인 모드
public enum GameMode
{
    NONE = 0,
    TRANNING,
    FREE,
    COUNT
}

// 트레이닝 모드
public enum TranningMode
{
    NONE = 0,
    STRAIGHT,
    DISTANCE,
    GRADIENT,
    ACTUAL,
    COUNT        
}

// 직선 연습 레벨
public enum StraightLevel
{
    NONE = 0,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    SEVEN,
    EIGHT,
    COUNT
}

// 거리 훈련 난이도
public enum DistanceLevel
{
    NONE = 0,
    EASY,
    NORMAL,
    HARD,
    COUNT
}

// 거리 훈련 방식
public enum DistanceMethod
{
    NONE = 0,
    FIXED,
    ROTATION,
    RANDOM,
    COUNT
}

// 기울기 훈련 환경
public enum GradientCondition
{
    NONE = 0,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    SEVEN,
    EIGHT,
    COUNT
}

// 기울기 훈련 레벨
public enum GradientLevel
{
    NONE = 0,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    COUNT
}

// 실전 컵 지정 방식
public enum ActualCupPoint
{
    NONE = 0,
    FIXED,
    RANDOM,
    COUNT
}

// 실전 시작 지점 방식
public enum ActualStartPoint
{
    NONE = 0,
    FIXED,
    RANDOM,
    COUNT
}

// 실전 경사 설정
public enum ActualGradient
{
    NONE = 0,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    COUNT
}

// 인포메이션 팝업 
public enum InfoPopupMode
{
    NONE = 0,
    STRAIGHT,
    DISTANCE,
    GRADIENT,
    ACTUAL,
    STRAIGHTSEEMORE,
    DISTANCESEEMORE,
    GRADIENTSEEMORE,
    ACTUALSEEMORE,
    COUNT
}

