function LargestPair(num) { 

    let largest = 0;
    let smallest = 9;
    let number = num.toString();
    var bigestTwoDigitNum; 
    
    while (num){
      let r = num % 10;
      largest = Math.max(r,largest);
      smallest = Math.min(r,smallest);
      num = parseInt(num) / 10;
    }
      largest = parseInt(largest.toPrecision(2));  
    
      for(let i=0;i<number.length-1;i++)
      {
        if(number[i] === largest.toString())
        {
          bigestTwoDigitNum = number[i]+number[i+1];
        }
      }
      return bigestTwoDigitNum;
    }
       
    // keep this function call here 
    // @ts-ignore
    console.log(LargestPair(readline()));