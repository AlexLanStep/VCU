function koeffs = solve_lsm(n,x,y,wts)
    
    str = strcat('poly',num2str(n));
    opts = fitoptions('Weights',wts);
    ftype = fittype(str);
    result = fit(x',y',ftype,opts);
    koeffs = coeffvalues(result)';

end