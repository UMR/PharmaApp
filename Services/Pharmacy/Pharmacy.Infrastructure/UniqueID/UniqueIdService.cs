/* This service is use to generate long type unique
 * As per the DateTime tick precision, this service should generate 100 unique id per second
 * To prevent the collution we have append the server version, thread id and sequence no
 * server version is 8 bit, thread id is 8 bit and sequence id is 8 bit
 * so the id is following
 * ticks (40 bit) + serverVersion (8 bit) + threadId (8 bit) + sequenceNo (8 bit)
 */

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

        if(sequenceNo >= maxSequenceNo)
        {
            sequenceNo = 1;
        }

        long ticks = DateTime.UtcNow.Ticks - startPeriod.Ticks;
        int threadId = Thread.CurrentThread.ManagedThreadId & 0xFF;

        long mask40BitOfTicks = ticks & 0x000000FFFFFFFFFF;         // mask to lowest 40 bit  

        long newId = mask40BitOfTicks << 24;    // Shift the mask to 20 bit
        newId |= (serverVersion & 0xFF) << 16;   // Convert the server version to 4-bit value and place into the position
        newId |= (threadId & 0xFF) << 8;        // Convert the threadId to 8-bit value and place into the position
        newId |= (sequenceNo & 0xFF);           // Convert the sequence number to 8-bit and place into the position

        return newId;
    }
}
