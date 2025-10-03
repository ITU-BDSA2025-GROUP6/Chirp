namespace Client_Test;

public class CheepServiceTest
{
    [Fact]
    public void getCheepsTest()
    {
        CheepService cs = new CheepService();
        Assert.Equal(2, cs.GetCheeps().Count());
        
        
    }
}