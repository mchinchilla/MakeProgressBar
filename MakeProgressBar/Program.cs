using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Path = System.IO.Path;

// Set the target date. You can change this to your preference.
// The prompt mentioned two options:
// 1. November 30, 2025
// 2. The start of 2026 (which is January 1, 2026)
var targetDate = new DateTime( 2025, 11, 30 );
// var targetDate = new DateTime(2026, 1, 1); // <-- Alternative target date

// --- 1. DATE CALCULATION ---

// To calculate a meaningful percentage, we need a start date for our "project".
// Let's use January 1st of the current year.
var startDate = new DateTime( DateTime.Today.Year, 1, 1 );
var today = DateTime.Today;

// Ensure our dates are logical
if ( today >= targetDate )
{
    Console.WriteLine( "The target date has already passed! Project complete." );
    return;
}

if ( today < startDate )
{
    Console.WriteLine( "The project start date is in the future. Progress is 0%." );
    startDate = today; // Adjust start date to today to avoid negative progress
}

// Calculate total duration and progress
var totalDaysInProject = ( targetDate - startDate ).TotalDays;
var daysElapsed = ( today - startDate ).TotalDays;
var daysRemaining = ( targetDate - today ).TotalDays;
var progressPercentage = ( daysElapsed / totalDaysInProject );

// --- 2. DISPLAY INFO IN CONSOLE ---

Console.WriteLine( "--- Progress Towards Target Date ---" );
Console.WriteLine( $"      Target Date: {targetDate:MMMM dd, yyyy}" );
Console.WriteLine( $"        Days Left: {Math.Ceiling( daysRemaining )}" );
Console.WriteLine( $"Progress Complete: {progressPercentage:P1}" ); // P1 format = 1 decimal place percentage
Console.WriteLine();

// --- 3. PRINT ASCII PROGRESS BAR ---

PrintAsciiProgressBar( progressPercentage, daysRemaining );

// --- 4. GENERATE PROGRESS IMAGE ---

try
{
    string outputPath = $"progress_{DateTime.Now:yyyyMMdd}.png";
    GenerateProgressImage( progressPercentage, daysRemaining, targetDate, outputPath );
    Console.WriteLine( $"\n✅ Image successfully generated at: {Path.GetFullPath( outputPath )}" );
}
catch ( Exception ex )
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine( $"\n❌ Error generating image: {ex.Message}" );
    Console.WriteLine( "Please ensure a font file (e.g., 'arial.ttf' or 'Roboto-Regular.ttf') is in the output directory." );
    Console.ResetColor();
}


// ============== METHOD DEFINITIONS ==============

static void PrintAsciiProgressBar( double percentage, double daysRemaining )
{
    Console.WriteLine( $"#SEVAN en {daysRemaining} días:" );
    int barWidth = 50;
    int filledWidth = ( int )( percentage * barWidth );

    Console.Write( "[" );
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write( new string( '█', filledWidth ) ); // The '█' character makes a solid bar
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write( new string( '-', barWidth - filledWidth ) );
    Console.ResetColor();
    Console.Write( $"] {percentage:P1}\n" );
}


static void GenerateProgressImage( double percentage, double daysRemaining, DateTime targetDate, string outputPath )
{
    // Define dimensions and colors inspired by your example
    int width = 800;
    int height = 450;
    Color backgroundColor = Color.ParseHex( "E1F5FE" ); // Light blue background
    Color primaryColor = Color.ParseHex( "03A9F4" ); // Main blue for progress
    Color textDarkColor = Color.ParseHex( "0277BD" ); // Darker blue for text
    Color white = Color.White;

    using var image = new Image<Rgba32>( width, height );

    // --- FONT SETUP ---
    // You must have a .ttf font file in your project's output directory.
    // Download "Roboto" from Google Fonts or use a system font like Arial.
    var fontCollection = new FontCollection();
    // Ensure you have a font file in your output directory (e.g., Roboto-Regular.ttf)
    // and its properties are set to "Copy to Output Directory".
    var fontFamily = fontCollection.Add( "Roboto-Regular.ttf" );
    var bigFont = fontFamily.CreateFont( 70, FontStyle.Bold );
    var mediumFont = fontFamily.CreateFont( 24, FontStyle.Regular );

    // The Drawing process
    image.Mutate( ctx =>
    {
        // Set background color
        ctx.Fill( backgroundColor );

        // --- 1. Draw the Circular Progress Indicator (left side) ---
        var circleCenter = new PointF( 160, height / 2f );
        float circleRadius = 110;
        float thickness = 25;

        // Draw the white "track" for the circle
        // CORRECTED LINE: Use Pens.Solid() instead of new Pen()
        ctx.Draw( Pens.Solid( white, thickness ), new EllipsePolygon( circleCenter, circleRadius ) );

        // Draw the blue progress arc
        // We start at -90 degrees (top of the circle) and sweep clockwise
        var progressArc = new PathBuilder()
            .AddArc( circleCenter, circleRadius, circleRadius, 0, -90, ( float )( 360 * percentage ) )
            .Build();

        // CORRECTED LINE: Use Pens.Solid() instead of new Pen()
        ctx.Draw( Pens.Solid( primaryColor, thickness ), progressArc );

        // Draw the percentage text inside the circle
        string percentText = $"{percentage:P0}"; // P0 = 0 decimal places
        var textOptions = new RichTextOptions( bigFont )
        {
            Origin = circleCenter,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        ctx.DrawText( textOptions, percentText, textDarkColor );

        // --- 2. Draw the Horizontal Progress Bar (top right) ---
        int barHeight = 40;
        var barPosition = new Point( 320, 120 );
        int barWidth = width - barPosition.X - 60;

        // Draw the white "track" for the bar
        ctx.Fill( white, new Rectangle( barPosition.X, barPosition.Y, barWidth, barHeight ) );

        // Draw the blue progress fill
        ctx.Fill( primaryColor, new Rectangle( barPosition.X, barPosition.Y, ( int )( barWidth * percentage ), barHeight ) );

        // --- 3. Draw Informational Text (bottom right) ---
        string daysLeftText = $"ZDM en {Math.Ceiling( daysRemaining )} dias, SE VAN!!!";
        string targetDateText = $"ALV el {targetDate:dd MMM, yyyy}";
        var infoTextOptions = new RichTextOptions( mediumFont )
        {
            Origin = new Point( barPosition.X, barPosition.Y + barHeight + 40 )
        };
        ctx.DrawText( infoTextOptions, daysLeftText, textDarkColor );

        // Move the origin down for the next line of text
        infoTextOptions.Origin = new Point( barPosition.X, barPosition.Y + barHeight + 80 );
        ctx.DrawText( infoTextOptions, targetDateText, textDarkColor );
    } );

    // Save the final image to a file
    image.Save( outputPath );
}