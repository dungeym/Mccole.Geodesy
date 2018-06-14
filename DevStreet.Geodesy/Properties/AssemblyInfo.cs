using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("McCole Limited")]
[assembly: AssemblyCopyright("Copyright © McCole Limited 2017. All rights reserved.")]
[assembly: AssemblyTrademark("DevStreet.co.uk")]
[assembly: AssemblyCulture("")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyTitle("DevStreet.Geodesy")]
[assembly: AssemblyProduct("DevStreet.Geodesy")]
[assembly: AssemblyDescription("DevStreet.Geodesy, the bulk of the logic in this library is based on the work of Chris Veness and Elmar de Koning. Published under the same MIT License where applicable.")]
[assembly: ComVisible(false)]
[assembly: Guid("552e97cf-a7ab-4f0a-904d-18668becc10c")]
[assembly: AssemblyVersion("1.0.18165.5")]
[assembly: AssemblyFileVersion("1.0.18165.5")]
/*
 * The bulk of the logic in this library is from 2 sources.
 *
 * The geodesic work was transcribed from JavaScript source by Chris Veness (C) 2005-2016
 * and published under the same MIT License.
 * https://www.movable-type.co.uk/scripts/latlong.html
 *
 * The polyline simplification work is based on the articles by Elmar de Koning.
 * It is not a transcription of his work.
 * http://psimpl.sourceforge.net/index.html
 *
 * http://www.contactandcoil.com/software/a-very-fast-tutorial-on-open-source-licenses/
 */