using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConferenceRegistration.Pages;

using ConferenceRegistration.Data;
using global::ConferenceRegistration.Data;

[BindProperties]
public class RegistrationModel(ConferenceRegistrationDbContext context) : PageModel

{
    public Participant Participant { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        context.Participants.Add(Participant);
        await context.SaveChangesAsync();
        return RedirectToPage("./Thanks", Participant);
    }
}