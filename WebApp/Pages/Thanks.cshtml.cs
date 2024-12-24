namespace ConferenceRegistration.Pages;
using ConferenceRegistration.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ThanksModel : PageModel
{
    public Participant Participant { get; set; } = new();
    public void OnGet(Participant participant)
    {
        Participant = participant;
    }
}