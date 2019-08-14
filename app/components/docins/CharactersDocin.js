import React from 'react'

import styles from '../../styles/docins/docins.characters.scss'

class CharactersDocin extends React.Component {
  render () {
    return (
      this.props.isVisible && (
        <div className={styles.charactersDocin}>
          <p>Characters:</p>
          <ul>
            <li>Evelynn</li>
            <li>James</li>
            <li>John</li>
            <li>Mara</li>
            <li>Ada</li>
            <li>Ethan</li>
          </ul>
        </div>
      )
    )
  }
}

export default CharactersDocin
