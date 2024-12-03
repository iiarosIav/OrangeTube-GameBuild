
public class MainStorage : Storage
{
    public static MainStorage Instance;

    void Awake()
    {
        Instance = this;
    }
}
