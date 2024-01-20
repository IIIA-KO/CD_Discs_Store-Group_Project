import React from 'react'
import {useEffect,useState} from 'react'
import CardListMusic from '../CardListMusic/CardListMusic';
const Music = () => {
  const [items, setItems] = useState([]);
  
   useEffect(() => {

   fetch("https://localhost:7117/Music/GetAll?skip=0").then(res=>res.json()).then(data => setItems(data)).catch(error => console.error(error));
  }, [])
  return (
    <div>
      <CardListMusic data={items}/>
    </div>
  )
}

export default Music

