namespace Pharmacy.Infrastructure.UniqueID;

public class UniqueIdService: IUniqueIdService
{
    private readonly DateTime startPeriod;
    private readonly uint serverVersion;
    private uint sequenceNo = 0;
    private const int maxSequenceNo = 255;

    public UniqueIdService()
    {
        startPeriod = new DateTime(2025, 1, 1);
        sequenceNo = 0;
        serverVersion = 1;
    }

    public long GetNextID()
    {
        sequenceNo++;

        if(sequenceNo > maxSequenceNo)
        {
            sequenceNo = 1;
        }

        long ticks = DateTime.UtcNow.Ticks - startPeriod.Ticks;

        long last48BitOfTicks = (ticks >> 16);              // Right shift to extract last 16 bits 

        long newId = last48BitOfTicks << 8;                 // Left shift 8 bit to append the server id
        newId |= serverVersion;                                  // Append server version

        newId = newId << 8;                                 // Left shift 8 bit to append sequence no
        newId |= sequenceNo;                                  // Append sequence no

        return newId;
    }
}
