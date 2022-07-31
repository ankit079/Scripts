function CamelCase(str:String) { 
    const re = /\s*W/g;
    const result = str.split(re);
    for(let i=0; i<result.length;i++)
    {
      if(i>0)
      {
        var newString = "";
      
        for(let j =0; j<result[i].length; j++)
        {       
            if(j===0)
            {
           newString += result[i].charAt(j).toUpperCase();  
            } 
            else
            {
            newString += result[i].charAt(j).toLowerCase();    
            }                
        }
            result[i] = newString;  
      }
      else
      {
        var newString = "";
        for(let j =0; j<result[i].length; j++)
        { 
          newString += result[i].charAt(j).toLowerCase(); 
        }
        result[i] = newString;
      }
    }
    return result; 
  }
     
  // keep this function call here 
  // @ts-ignore
  console.log(CamelCase(readline()));