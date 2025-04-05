/* This service is use to generate long type unique integer (id)
 * As per the DateTime tick precision, this service should generate 100 unique id per second
 * To prevent the collusion we have append the server version, thread id and sequence no
 * Server version is 8 bit, thread id is 8 bit and sequence id is 8 bit
 * so the id is following
 * ticks (40 bit) + serverVersion (8 bit) + threadId (8 bit) + sequenceNo (8 bit)
 */

/*
 *  Suppose we have a tick = 638785044640557663, the bit manupulation will be followings
  
  0000 1000 1101 1101 0110 1011 1001 1101 0001 1001 1010 0001 1110 1010 0101 1111	=>	638785044640557663  => binary of the tick
  0000 0000 0000 0000 0000 0000 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111	=>	0x000000FFFFFFFFFF  => mask bit (to get the lowes 40 bit (LSB))
  ----------------------------------------------------------------------------------------------------------------------------------------------------	 
  0000 0000 0000 0000 0000 0000 1001 1101 0001 1001 1010 0001 1110 1010 0101 1111 	=> ticks & 0x000000FFFFFFFFFF           => After (&) operation we have 40 LSB
                                                                                    => Before shift 24 bit, we set the 40th bit to 0 to avoid the negative number. Or MSB is set to 0
  0100 1110 1000 1100 1101 0000 1111 0101 0010 1111 1000 0000 0000 0000 0000 0000	=> (ticks & 0x000000FFFFFFFFFF) << 24   => Shift the masked bit to append server id, thread id and sequence number
																	    0000 0001	=> ServerVersion & 0xFF                 => Make the serverVersion as 8 bit int. Server Version is 1 in this case.
													     0001 0000 0000 0000 0000	=> (ServerVersion & 0xFF) << 16         => Shift the serverVersion to place it's position
  0100 1110 1000 1100 1101 0000 1111 0101 0010 1111 1000 0001 0000 0000 0000 0000	=> newID | (ServerVersion & 0xFF) <<16  => Perform (|) operation to place the server version in the ID.
   
  the thread id and sequence number will be placed it's positon similarly
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
        startPeriod = new DateTime(2000, 1, 1);
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

        long mask40BitOfTicks = ticks & 0x000000FFFFFFFFFF;         // mask to get only the lowest 40 bit

        long onlyThe39BitIsOff = ~((long)1 << 39);                  // Prepare mask to set the 40th bit as 0. 
                                                                    // All bit of onlyThe39bitIsOff variable is 1, except the 40th bit

        mask40BitOfTicks &= onlyThe39BitIsOff;                      // Set the 40th bit to 0 before shift. To make sure that the MSB is always 0.

        long newId = mask40BitOfTicks << 24;    // Shift the mask to 24 bit
        newId |= (serverVersion & 0xFF) << 16;  // Convert the server version to 8-bit value and place into the position
        newId |= (threadId & 0xFF) << 8;        // Convert the threadId to 8-bit value and place into the position
        newId |= (sequenceNo & 0xFF);           // Convert the sequence number to 8-bit and place into the position

        return newId;
    }
}
