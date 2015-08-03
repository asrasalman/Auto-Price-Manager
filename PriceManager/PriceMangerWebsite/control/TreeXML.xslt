<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>
  <!-- Find the root node called Menus then convert it to <UL> </UL> HTMLTags
       and call MenuListing for its children -->
  <xsl:template match="/Trees">
    <ul>
      <xsl:call-template name="TreeListing" />
    </ul>
  </xsl:template>

  <!-- Allow for recusive child node processing -->
  <xsl:template name="TreeListing">
    <xsl:apply-templates select="Tree" />
  </xsl:template>

  <xsl:template match="Tree">
    <li>
      <xsl:attribute name="id">
        <xsl:value-of select="Id"/>
      </xsl:attribute>
      <xsl:attribute name="parentId">
        <xsl:value-of select="ParentId"/>
      </xsl:attribute>
      <xsl:attribute name="custom1">
        <xsl:value-of select="Custom1"/>
      </xsl:attribute>
      <xsl:attribute name="custom2">
        <xsl:value-of select="Custom2"/>
      </xsl:attribute>
      <xsl:attribute name="custom3">
        <xsl:value-of select="Custom3"/>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:choose>
          <xsl:when test="Checked = 'true'">
            jstree-checked
          </xsl:when>
          <xsl:otherwise>
            jstree-unchecked
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <a>
        <!-- Convert Menu child elements to <li> <a> html tags  and attributes inside a tag -->
        <xsl:attribute name="href">
          <xsl:value-of select="NavigateUrl"/>
        </xsl:attribute>
        <xsl:value-of select="Text"/>
      </a>
      <!-- Recursively call MenuListing for child menu nodes -->

      <xsl:if test="count(Tree) > 0">
        <ul>
          <xsl:call-template name="TreeListing" />
        </ul>
      </xsl:if>
    </li>
  </xsl:template>
</xsl:stylesheet>
