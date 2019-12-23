INSERT INTO dbo.Tank (Name,Stage,Type,PlaceOfOrigin,Manufacturer,ServiceStartYear,Weight,Length,
                      Width,Height,Crew,Armor,MainArmament,SecondaryArmament,Engine,Power,
                      Transmission,Suspension,GroundClearance,FuelCapacity,Range,
                      Speed,Description,ImagePath,ImageFile)
VALUES ('M1A2 Abrams', 4, 'Main battle tank', 'United States', 'Lima Army Tank Plant (since 1980);Detroit Arsenal Tank Plant (1982–1996)', 1980, 65, 9.77, 3.66, 2.44, 4, 
        'depleted uranium mesh-reinforced composite armor', '120 mm L/44 M256A1 smoothbore gun (42 rounds)', 
        '1 × .50-caliber (12.7 mm) M2HB heavy machine gun with 900 rounds, 2 × 7.62 mm (.308 in) M240 machine guns with 10,400 rounds (1 pintle-mounted, 1 coaxial)', 
        'Honeywell AGT1500C multi-fuel turbine engine 1,500 shp (1,120 kW)',1100, 'Allison DDA X-1100-3B', 
	'High-hardness-steel torsion bars with rotary shock absorbers', 0.43, 1900, 426, 67, 
	'The M1 Abrams is an American third-generation main battle tank. It is named after General Creighton Abrams, former Army chief of staff, commander of United States military forces in the Vietnam War from 1968 to 1972, and a tank commander during World War II. Highly mobile, designed for modern armored ground warfare,[14] the M1 is well armed and heavily armored. Notable features include the use of a powerful multifuel turbine engine, the adoption of sophisticated composite armor, and separate ammunition storage in a blow-out compartment for crew safety. Weighing nearly 68 short tons (almost 62 metric tons), it is one of the heaviest main battle tanks in service.',
        '~/Content/data/', 'M1-Abrams.png');
