function koeffs = solve_lsm(n,x,y,wts)
    
    str = strcat('poly',num2str(n));
    opts = fitoptions('Weights',wts);
    ftype = fittype(str);
    y1=y;
    result = fit(x',y',ftype,opts);
    koeffs = coeffvalues(result)';
    t=1:length(y1);
    plot(t, y1);
    pause(5);
end