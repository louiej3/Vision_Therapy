using SQLite4Unity3d;

public class Address
{

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string street { get; set; }
    public string city { get; set; }
    public int zip { get; set; }
    public string state { get; set; }

    public override string ToString()
    {
        return string.Format("[Address: Id={0}, street={1},  city={2}, zip={3}, state={4}]", id, street, city, zip, state);
    }
}
