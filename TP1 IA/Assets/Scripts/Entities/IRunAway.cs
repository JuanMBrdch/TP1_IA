public interface IRunAway
{

    void RunAway();

    bool IsRunningAway { get; set; }

    int RunAwayDuration { get; set; }
}
