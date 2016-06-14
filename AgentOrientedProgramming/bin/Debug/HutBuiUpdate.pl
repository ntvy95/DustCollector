:- dynamic dirty/2.
:- dynamic obstacle/3.
:- dynamic wasin/2.
:- dynamic wasweight/3.
:- dynamic choosed/1.
:- dynamic wasleftmoves/1.
:- dynamic discovered/2.
:- dynamic done/1.
:- dynamic wasfacing/1.
:- dynamic wastimePassed/1.

in(0,0) :- done(start).
leftmoves(0) :- done(start), assert(wasleftmoves(0)).
facing(0) :- done(start).
discover(0,0) :- done(start), assert(discovered(0,0)).
timePassed(0) :- done(start).

facing(Temp):- wasfacing(A), Temp is A+90, done(turn90).
facing(A):-wasfacing(A),done(forward).
in(Temp1, Temp2):- wasin(X,Y), wasfacing(A), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), done(forward).
weight(X,Y,Temp):- wasin(X,Y), (wasweight(X,Y,W);W is 0), Temp is W+1, done(forward).
discover(Temp1, Temp2):- (done(forward);done(start)),in(X,Y), facing(M), (A is M;A is M+90;A is M+180;A is M+270), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), not(obstacle(Temp1, Temp2, static)), not(discovered(Temp1, Temp2)).
leftmoves(TempK):- (done(forward);done(start)), in(X,Y), facing(M), (A is M;A is M+90;A is M+180;A is M+270), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), not(obstacle(Temp1, Temp2, static)), not(discovered(Temp1, Temp2)), wasleftmoves(K), TempK is K + 1, retract(wasleftmoves(K)), assert(wasleftmoves(TempK)).
leftmoves(TempK):- in(X,Y), not(wasweight(X,Y,_)), wasleftmoves(K), TempK is K - 1, retract(wasleftmoves(K)), assert(wasleftmoves(TempK)), done(forward).
timePassed(Temp):- wastimePassed(T), Temp is T+1, done(stop).
