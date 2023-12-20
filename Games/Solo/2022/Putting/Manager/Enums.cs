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

    // Ʈ���̴� ��� �ѱ۷� ����
    public string TranningModeToKor(int mode)
    {
        string kor = "";
        switch (mode)
        {
            case (int)TranningMode.STRAIGHT:
                kor = "���� �Ʒ�";
                break;
            case (int)TranningMode.DISTANCE:
                kor = "�Ÿ� �Ʒ�";
                break;
            case (int)TranningMode.GRADIENT:
                kor = "���� �Ʒ�";
                break;
            case (int)TranningMode.ACTUAL:
                kor = "���� �Ʒ�";
                break;
        }
        return kor;
    }

    // ���� �Ʒ� �ܰ� �� ����
    public string StraightTitleToKor(int level)
    {
        string kor = "";
        switch(level)
        {
            case (int)StraightLevel.ONE:
                kor = "���� ���̵� ������ ���� ������ �Ʒ��ϼ���.";
                break;
            case (int)StraightLevel.TWO:                
            case (int)StraightLevel.THREE:                
            case (int)StraightLevel.FOUR:
                kor = "�� ���� ���̵� ������ ���� ������ �Ʒ��ϼ���.";
                break;
            case (int)StraightLevel.FIVE:                
            case (int)StraightLevel.SIX:                
            case (int)StraightLevel.SEVEN:
                kor = "��� �� ���� ���̵� ������ ���� ������ �Ʒ��ϼ���.";
                break;
            case (int)StraightLevel.EIGHT:                
                kor = "������ ������ ������ �Ʒ��ϼ���.";
                break;
        }
        return kor;
    }
    
    // ���� �Ʒ� �ܰ� �� ����
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
                kor = "���� ���̵� ������ ������ ����� �ʵ���" +
                                "\n������ �����ؾ� �մϴ�.";
                break;

            case (int)StraightLevel.EIGHT:
                kor = "���� ���̵� ���� ������ ����� �ʰ�," +
                                "\nǥ�õ� �ſ� ���� ������ ������ �����ؾ� �մϴ�.";
                break;
        }
        return kor;
    }

    // ���� �Ʒ� ��Ģ
    public string StraightRule(int level)
    {
        // �ӽ÷� ���� (����, ���� ���� (���� cm))
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

    // �Ÿ� �Ʒ� ���̵� �ѱ۷� ����
    public string DistanceLevelToKor(int mode)
    {
        string kor = "";
        switch (mode)
        {
            case (int)DistanceLevel.EASY:
                kor = "����";
                break;
            case (int)DistanceLevel.NORMAL:
                kor = "����";
                break;
            case (int)DistanceLevel.HARD:
                kor = "�����";
                break;            
        }
        return kor;
    }

    // �Ÿ� �Ʒ� ��� �ѱ۷� ����
    public string DistanceMethodToKor(int method)
    {
        string kor = "";
        switch(method)
        {
            case (int)DistanceMethod.FIXED:
                kor = "����";
                break;
            case (int)DistanceMethod.ROTATION:
                kor = "��ȯ";
                break;
            case (int)DistanceMethod.RANDOM:
                kor = "����";
                break;
        }
        return kor;
    }

    // �Ÿ� �Ʒ� ��Ŀ� ���� �Ʒ� ����
    public string DistanceInfoToKor(int method)
    {
        string kor = "";
        switch (method)
        {
            case (int)DistanceMethod.FIXED:
                kor = "�������� ��ǥ�� �����ؼ� �Ʒ��մϴ�.";
                break;
            case (int)DistanceMethod.ROTATION:
                kor = "���� �� ��ǥ ������ ���������� ����˴ϴ�.";
                break;
            case (int)DistanceMethod.RANDOM:
                kor = "���� �� ��ǥ ������ �����ϰ� ����˴ϴ�.";
                break;
        }
        return kor;
    }

    // �Ÿ� �Ʒ� ��Ģ
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

    // ���� �Ʒ� ȯ�� �� ����
    public string GradientTitleToKor(int condition)
    {
        string kor = "";
        switch(condition)
        {
            case (int)GradientCondition.ONE:
                kor = "1�ܰ� : ������ ���⸦ �Ʒ��մϴ�.";
                break;
            case (int)GradientCondition.TWO:
                kor = "2�ܰ� : �������� �������� �������� ���⸦ �Ʒ��մϴ�.";
                break;
            case (int)GradientCondition.THREE:
                kor = "3�ܰ� : �������� �������� �������� ���⸦ �Ʒ��մϴ�.";
                break;
            case (int)GradientCondition.FOUR:
                kor = "4�ܰ� : ������ ���⸦ �Ʒ��մϴ�.";
                break;
            case (int)GradientCondition.FIVE:
                kor = "5�ܰ� : ������, ���� ���⸦ �Բ� �Ʒ��մϴ�.";
                break;
            case (int)GradientCondition.SIX:
                kor = "6�ܰ� : ������, ���� ���⸦ �Բ� �Ʒ��մϴ�.";
                break;
            case (int)GradientCondition.SEVEN:
                kor = "7�ܰ� : ������, ���� ���⸦ �Բ� �Ʒ��մϴ�.";
                break;
            case (int)GradientCondition.EIGHT:
                kor = "8�ܰ� : ������, ���� ���⸦ �Բ� �Ʒ��մϴ�.";
                break;
        }
        return kor;
    }

    // ���� �Ʒ� ȯ�� �� ���� ����
    public string GradientEachInfoToKor(int condition)
    {
        string kor = "";
        switch (condition)
        {
            case (int)GradientCondition.ONE:
                kor = "�� ���� ������";
                break;
            case (int)GradientCondition.TWO:
                kor = "���� ���� ������";
                break;
            case (int)GradientCondition.THREE:
                kor = "���� ���� ������";
                break;
            case (int)GradientCondition.FOUR:
                kor = "�� ���� ������";
                break;
            case (int)GradientCondition.FIVE:
                kor = "�� ������, ���� ������";
                break;
            case (int)GradientCondition.SIX:
                kor = "�� ������, ���� ������";
                break;
            case (int)GradientCondition.SEVEN:
                kor = "�� ������, ���� ������";
                break;
            case (int)GradientCondition.EIGHT:
                kor = "�� ������, ���� ������";
                break;
        }
        return kor;
    }

    // ���� �Ʒ� �� ����Ʈ
    public string ActualCupToKor(int method)
    {
        string kor = "";
        switch(method)
        {
            case (int)ActualCupPoint.FIXED:
                kor = "����";
                break;
            case (int)ActualCupPoint.RANDOM:
                kor = "����";
                break;
        }
        return kor;
    }

    // ���� �Ʒ� ��ŸƮ ����Ʈ
    public string ActualStartToKor(int method)
    {
        string kor = "";
        switch (method)
        {
            case (int)ActualStartPoint.FIXED:
                kor = "����";
                break;
            case (int)ActualStartPoint.RANDOM:
                kor = "����";
                break;
        }
        return kor;
    }

    // ���� �Ʒ� ����
    public string ActualGradientToKor(int level)
    {
        string kor = "";
        switch (level)
        {
            case (int)ActualGradient.NONE:
                kor = "����";
                break;
            case (int)ActualGradient.ONE:
                kor = "1�ܰ�";
                break;
            case (int)ActualGradient.TWO:
                kor = "2�ܰ�";
                break;
            case (int)ActualGradient.THREE:
                kor = "3�ܰ�";
                break;
            case (int)ActualGradient.FOUR:
                kor = "4�ܰ�";
                break;
            case (int)ActualGradient.FIVE:
                kor = "5�ܰ�";
                break;
        }
        return kor;
    }
}


// ���� ���
public enum GameMode
{
    NONE = 0,
    TRANNING,
    FREE,
    COUNT
}

// Ʈ���̴� ���
public enum TranningMode
{
    NONE = 0,
    STRAIGHT,
    DISTANCE,
    GRADIENT,
    ACTUAL,
    COUNT        
}

// ���� ���� ����
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

// �Ÿ� �Ʒ� ���̵�
public enum DistanceLevel
{
    NONE = 0,
    EASY,
    NORMAL,
    HARD,
    COUNT
}

// �Ÿ� �Ʒ� ���
public enum DistanceMethod
{
    NONE = 0,
    FIXED,
    ROTATION,
    RANDOM,
    COUNT
}

// ���� �Ʒ� ȯ��
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

// ���� �Ʒ� ����
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

// ���� �� ���� ���
public enum ActualCupPoint
{
    NONE = 0,
    FIXED,
    RANDOM,
    COUNT
}

// ���� ���� ���� ���
public enum ActualStartPoint
{
    NONE = 0,
    FIXED,
    RANDOM,
    COUNT
}

// ���� ��� ����
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

// �������̼� �˾� 
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

