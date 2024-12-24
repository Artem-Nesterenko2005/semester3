using System;

namespace ConferenceRegistration.Data;
public class Participant
{
    public int ParticipantId { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public bool IsSpeaker { get; set; }
}
