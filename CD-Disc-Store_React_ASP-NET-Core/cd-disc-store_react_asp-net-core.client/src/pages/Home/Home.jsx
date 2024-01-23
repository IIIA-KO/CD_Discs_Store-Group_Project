import React from 'react'
import { useEffect, useState } from 'react'
import CardList from '../CardList/CardList';
const Home = () => {
  const [items, setItems] = useState([]);

  useEffect(() => {

    fetch("https://localhost:7117/Film/GetAll?skip=0").then(res => res.json()).then(data => setItems(data)).catch(error => console.error(error));
  }, [])


  return (
    <div>
      <p>home</p>
      <CardList data={items} />
    </div>
  )
}

export default Home