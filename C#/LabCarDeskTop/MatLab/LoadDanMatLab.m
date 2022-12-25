function y = LoadDanMatLab(path)
  newData1 = load('-mat', path);
  vars = fieldnames(newData1);
  y = newData1.(vars{1});
end

