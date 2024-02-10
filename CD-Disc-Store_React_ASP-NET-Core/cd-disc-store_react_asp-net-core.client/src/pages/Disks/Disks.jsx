import React, { useEffect, useState } from 'react'
import DiskSearch from '../../DiskSearch/DiskSearch';

const Disks = () => {
  const [items, setItems] = useState([]);

  useEffect(() => {

    fetch("https://localhost:7117/Discs/GetAll?skip=0").then(res => res.json()).then(data => setItems(data)).catch(error => console.error(error));
  }, [])
  return (
    <div>
      <DiskSearch disks={items} />
    </div>
  )
}

export default Disks
