/// <summary>
/// Summary �ϥΪ̪��Ҧ��������
/// </summary>
public class CSessionInfo
{
    public string OrgID;                //���c�N��
    public string OrgName;              //���c�W��
    public string UserID;               //�ϥΪ̥N��
    public string UserName;             //�ϥΪ̦W��
    public string DeptID;               //����ID
    public string DeptName;             //��������W��
    public string GroupID;              //�ϥΪ��v���s��ID
    public string GroupName;            //�ϥΪ��v���s��
    public string GroupArea;            //�ϥΪ��v���d��

    public CSessionInfo()
	{
	}    

    public void Clear()
	{
        OrgID = "";
        OrgName = "";
        UserID = "";
        UserName = "";
        DeptID = "";
        DeptName = "";
        GroupID = "";
        GroupName = "";
        GroupArea= "";
	}    
}
