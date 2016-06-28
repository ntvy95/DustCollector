:- dynamic obstacle/3.
:- dynamic wasin/2.
:- dynamic wasweight/3.
:- dynamic wasleftmoves/1.
:- dynamic discovered/2.
:- dynamic done/1.
:- dynamic wasfacing/1.

in(0,0) :- done(start).
leftmoves(0) :- done(start), assert(wasleftmoves(0)).
facing(0) :- done(start).
discover(0,0) :- done(start), assert(discovered(0,0)).
timePassed(0) :- done(start).

facing(Temp):- wasfacing(A), Temp is A+90, done(turn90).
facing(A):-wasfacing(A),done(forward).
in(Temp1, Temp2):- wasin(X,Y), wasfacing(A), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), done(forward).
weight(Temp1,Temp2,inf) :- in(X,Y), (A is 0;A is 90;A is 180;A is 270), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), obstacle(Temp1,Temp2,static), (done(forward);done(start)).
weight(X,Y,Temp) :- wasin(X,Y), not(useful(X,Y)), (wasweight(X,Y,W);not(wasweight(X,Y,_)),W is 1), Temp is W*10, done(forward).
weight(X,Y,Temp):- wasin(X,Y), useful(X,Y), (wasweight(X,Y,W);not(wasweight(X,Y,_)),W is 0), Temp is W+1, done(forward).
discover(Temp1, Temp2):- (done(forward);done(start)),in(X,Y), facing(M), (A is M;A is M+90;A is M+180;A is M+270), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), not(obstacle(Temp1, Temp2, static)), not(discovered(Temp1, Temp2)).
leftmoves(TempK):- (done(forward);done(start)), in(X,Y), facing(M), (A is M;A is M+90;A is M+180;A is M+270), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), not(obstacle(Temp1, Temp2, static)), not(discovered(Temp1, Temp2)), retract(wasleftmoves(K)), TempK is K + 1, assert(wasleftmoves(TempK)).
leftmoves(TempK):- in(X,Y), not(wasweight(X,Y,_)), retract(wasleftmoves(K)), TempK is K - 1, assert(wasleftmoves(TempK)), done(forward).
useful(X,Y) :- (A is 0;A is 90;A is 180;A is 270), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), not(wasweight(Temp1,Temp2,_)).
