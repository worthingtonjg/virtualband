
using System;

public class PlayerInfo
{
    public string Id { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }
    public float ForwardX { get; set; }
    public float ForwardY { get; set; }
    public float ForwardZ { get; set; }


    public override string ToString()
    {
        return string.Format("PlayerInfo Id={0} PositionX={1} PositionY={2} PositionZ={3} ForwardX={4} ForwardY={5} ForwardZ={6}", 
            Id, PositionX, PositionY, PositionZ, ForwardX, ForwardY, ForwardZ);
    }

    private bool NearEquals(float f1, float f2)
    {
        return (Math.Abs(f1 - f2) < 0.00f);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        PlayerInfo playerInfo = obj as PlayerInfo;
        if (playerInfo == null)
        {
            return false;
        }
        return Id.Equals(playerInfo.Id) 
            && NearEquals(PositionX, playerInfo.PositionX) 
            && NearEquals(PositionY, playerInfo.PositionY) 
            && NearEquals(PositionZ, playerInfo.PositionZ)
            && NearEquals(ForwardX, playerInfo.ForwardX) 
            && NearEquals(ForwardY, playerInfo.ForwardY) 
            && NearEquals(ForwardZ, playerInfo.ForwardZ);
    }

}

