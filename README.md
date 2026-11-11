Projekt: Minu ülesanded

Programmeerimiskeel: C#
Tehnoloogia: WPF (Windows Presentation Foundation)
Arenduskeskkond: Visual Studio 2026

1. Kellele rakendus on mõeldud ja millist probleemi see lahendab?

Minu rakendus „Minu ülesanded” on mõeldud õpilastele, kes soovivad oma koduseid ülesandeid paremini planeerida. Õpilastel on tihti palju koduseid ülesandeid erinevates õppeainetes ning mõnikord võivad nad unustada, mida ja mis ajaks on vaja teha. See rakendus aitab hoida kõik ülesanded ühes kohas, jälgida tähtaegu ja näha, millised ülesanded on juba tehtud.

2. Põhifunktsioonid ning vajalikud andmed

Rakendusel on järgmised funktsioonid:

1. Ülesande lisamine – kasutaja saab sisestada uue koduse ülesande.
2. Õppeaine valimine – kasutaja saab valida õppeaine, näiteks matemaatika, eesti keel või inglise keel.
3. Prioriteedi määramine – ülesandele saab määrata kõrge, keskmise või madala prioriteedi.
4. Tähtaja valimine – kasutaja saab valida kuupäeva, millal ülesanne peab valmis olema.
5. Ülesande tehtuks märkimine – kasutaja saab märkida ülesande tehtuks.
6. Ülesande kustutamine – kasutaja saab eemaldada ülesande nimekirjast.
7. Ülesannete filtreerimine – kasutaja saab vaadata kõiki, tehtud või tegemata ülesandeid.
8. Statistika kuvamine – rakendus näitab ülesannete koguarvu ning tehtud ja tegemata ülesannete arvu.
9. Andmete salvestamine – rakendus salvestab ülesanded automaatselt JSON-faili. Pärast programmi sulgemist ja uuesti avamist jäävad ülesanded alles.

Vajalikud andmed on ülesande nimi, õppeaine, prioriteet, tähtaeg ja ülesande staatus (tehtud või tegemata).

3. Reeglid, mida rakendus peab kontrollima

Reegel 1: Ülesande nimi ei tohi olla tühi. Kui kasutaja proovib lisada ülesannet ilma nimeta, kuvab rakendus veateate „Palun sisesta ülesande nimi!” ja ülesannet ei lisata.

Reegel 2: Kui kasutaja märgib ülesande tehtuks, peab rakendus uuendama statistikat.

Reegel 3: Kui kasutaja kustutab ülesande, peab see kaduma nimekirjast ja ülesannete koguarv peab vähenema.

Reegel 4: Filtreerimisel peab rakendus näitama ainult valitud staatusega ülesandeid.

4. Kokkuvõte

Minu rakendus aitab õpilastel oma koolitöid paremini korraldada ja aega planeerida. Rakendus on lihtne ja mugav kasutada. Sellel on roosa kujundus ning kõik vajalikud funktsioonid koduste ülesannete haldamiseks. Rakendus on loodud C# programmeerimiskeeles ja WPF-tehnoloogiaga.
